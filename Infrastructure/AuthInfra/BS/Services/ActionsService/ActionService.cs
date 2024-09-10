using BS.CustomExceptions.Common;
using BS.Services.ActionsService.Models;
using BS.Services.ActionsService.Models.Request;
using BS.Services.ActionsService.Models.Response;
using BS.Services.RoleService.Models.Response;
using DA;
using DM.DomainModels;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.ActionsService
{
    public class ActionService : IActionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INatsService _natsService;

        public ActionService(IUnitOfWork unitOfWork, INatsService natsService)
        {
            _unitOfWork = unitOfWork;
            _natsService = natsService ?? throw new ArgumentNullException(nameof(natsService));
        }

        public async Task<bool> AddAction(RequestAddAction request, string userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The request cannot be null.");
            }

            var actionExists = await _unitOfWork.action.AnyAsync(cancellationToken, a => a.Name == request.Name);
            if (actionExists.Data == true)
            {
                throw new RecordAlreadyExistException($"Action {request.Name} already exists.");
            }


            var entity = request.ToDomain(userId);

            if (entity == null)
            {
                throw new ArgumentException("The request is invalid and could not be converted to a domain entity.", nameof(request));
            }

            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            entity.IsArchived = false;
            entity.IsActive = true;

            await _unitOfWork.action.AddAsync(entity, userId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            
            /// NATS Publisher Usage
            var message = $"Action {request.Name} has been added by user {userId}.";
            await _natsService.PublishAsync("action_updates", "action.*", message);

            return true;
        }

        public async Task<bool> DeleteAction(string actionId, string userId, CancellationToken cancellationToken)
        {
            if (actionId == null)
            {
                throw new ArgumentNullException("actionId can not be null or empty");
            }

            var setterResult = await _unitOfWork.action.UpdateOnConditionAsync(
            // 1st param: matching condition
            x => x.IsActive == true && x.Id == actionId,
            // 2nd param: set the updated value
            x => x.SetProperty((Func<Actions, string?>)(y => y.Id), (string?)null)
                  .SetProperty((Func<Actions, string>)(y => y.UpdatedBy), userId)
                  .SetProperty((Func<Actions, DateTime>)(y => y.UpdatedDate), DateTime.UtcNow)
                  , cancellationToken);

            if (setterResult == null)
            {
                throw new InvalidOperationException("The update operation did not return a result.");
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteActions(string[] actionIds, string userId, CancellationToken cancellationToken)
        {
            if (actionIds == null || actionIds.Length == 0)
            {
                throw new ArgumentException("Action IDs cannot be null or empty.", nameof(actionIds));
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            var result = await _unitOfWork.action.GetAllAsync(cancellationToken);

            if (result == null || !result.Status || result.Data == null)
            {
                throw new InvalidOperationException("Could not retrieve Actions list");
            }

            var affectedActions = result.Data.ToList();

            foreach (var actions in affectedActions)
            {
                actions.IsActive = false;
                actions.UpdatedBy = userId;
                actions.UpdatedDate = DateTime.UtcNow;
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }

        public async Task<List<ResponseGetAllActionDetails>> GetAllAction(string userId, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.action.GetAllAsync(cancellationToken);
            List<ResponseGetAllActionDetails> response = new List<ResponseGetAllActionDetails>();

            if (result.Status)
            {
                foreach (var record in result.Data)
                {
                    if (record.Name != null && record.IsActive == true)
                    {
                        response.Add(new ResponseGetAllActionDetails()
                        {
                            Name = record.Name
                        });
                    }
                }
                return response;
            }
            else
            {
                throw new InvalidOperationException("Failed to retrieve Actions data.");
            }
        }

        public async Task<ResponseGetAllActionDetails> GetActionById(string actionId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(actionId))
            {
                throw new ArgumentException("Role ID cannot be null or empty.", nameof(actionId));
            }

            var actionsList = await _unitOfWork.action.GetAsync(cancellationToken, x => x.Id == actionId && x.IsActive);

            if (actionsList == null)
            {
                throw new RecordNotFoundException($"User role with ID {actionId} not found.");
            }

            var actions = actionsList.Data.FirstOrDefault();

            if (actions != null && actions.Name != null)
            {
                return new ResponseGetAllActionDetails()
                {
                    Name = actions.Name
                };
            }
            else
            {
                throw new RecordNotFoundException("ActionId not found");
            }
        }

        public async Task<bool> AppendActionTag(RequestAppendActionTag request, string userId, CancellationToken cancellationToken)
        {
            if (request.actionId == null || request.tagToAppend == null)
            {
                throw new ArgumentNullException("actionId or Tag to Append can not be null");
            }

            var existingAction = await _unitOfWork.action.GetByIdAsync(request.actionId, cancellationToken);
            if(existingAction.Data == null)
            {
                throw new RecordNotFoundException(existingAction.Message);
            }

            var actionData = existingAction.Data;
            var currentTags = actionData.GetActionTag();
            // IF tag is empty then set it as tag in req ELSE append the tag after comma
            var updatedTags = string.IsNullOrWhiteSpace(currentTags) ? request.tagToAppend : $"{currentTags},{request.tagToAppend}";

            var updateStatus = await _unitOfWork.action.UpdateOnConditionAsync(
                x => x.IsActive && x.Id == request.actionId,
                x => x.SetProperty(y => y.Tags, updatedTags)
                      .SetProperty(y => y.UpdatedBy, userId)
                      .SetProperty(y => y.UpdatedDate, DateTime.UtcNow),
                cancellationToken);

            if(updateStatus.Result == false)
            {
                throw new InvalidOperationException("could not update tags");
            }

            return updateStatus.Result;
        }


        public async Task<bool> RemoveActionTag(RequestRemoveActionTag request, string userId, CancellationToken cancellationToken)
        {
            var existingAction = await _unitOfWork.action.GetByIdAsync(request.actionId, cancellationToken);
            if (existingAction.Data == null)
            {
                throw new RecordNotFoundException("Action with such ID doesn't exist");
            }

            var actionData = existingAction.Data;
            var individualTags = actionData.GetActionTag().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var individualTagsWithTagToTrimRemoved = individualTags.Where(tag => tag != request.tagToRemove);
            var updatedTags = string.Join(",", individualTagsWithTagToTrimRemoved);

            var updateStatus = await _unitOfWork.action.UpdateOnConditionAsync(
                x => x.IsActive && x.Id == request.actionId,
                x => x.SetProperty(y => y.Tags, updatedTags)
                      .SetProperty(y => y.UpdatedBy, userId)
                      .SetProperty(y => y.UpdatedDate, DateTime.UtcNow),
                cancellationToken);

            if (updateStatus.Result == false)
            {
                throw new InvalidOperationException("could not remove tags");
            }

            return updateStatus.Result;
        }

    }
}
