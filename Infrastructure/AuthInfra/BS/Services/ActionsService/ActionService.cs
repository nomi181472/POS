﻿using BS.CustomExceptions.Common;
using BS.Services.ActionsService.Models;
using BS.Services.ActionsService.Models.Request;
using BS.Services.ActionsService.Models.Response;
using BS.Services.RoleService.Models.Response;
using DA;
using DA.Models.RepoResultModels;
using DM.DomainModels;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grpc.Core.Metadata;

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
            if(string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentNullException("Name can't be null");
            }

            if (string.IsNullOrWhiteSpace(request.Tag))
            {
                throw new ArgumentNullException("Tag can't be null");
            }

            var entity = request.ToDomain(userId);
            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            entity.IsArchived = false;
            entity.IsActive = true;

            await _unitOfWork.action.AddAsync(entity, userId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            // COMMENT OUT WHEN NATS WORKING IN DOCKER-COMPOSE
            //var message = $"Action {request.Name} has been added by user {userId}.";
            //await _natsService.PublishAsync("action_updates", "action.*", message);

            return true;
        }



        public async Task<bool> AddListOfActions(RequestAddListOfActions request, string userId, CancellationToken cancellationToken)
        {
            if (request == null || request.Actions == null || !request.Actions.Any())
            {
                throw new ArgumentNullException("Actions can't be null or empty");
            }

            foreach (var action in request.Actions)
            {
                if (string.IsNullOrWhiteSpace(action.Name) || string.IsNullOrWhiteSpace(action.Tag))
                {
                    throw new ArgumentNullException("Name and Tag can't be null");
                }

                var entity = action.ToDomain(userId);
                entity.UpdatedBy = userId;
                entity.UpdatedDate = DateTime.Now;
                entity.IsArchived = false;
                entity.IsActive = true;

                await _unitOfWork.action.AddAsync(entity, userId, cancellationToken);
            }

            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }



        public bool IsActionsAvailable(string name)
        {
            return _unitOfWork.action.Any(a => a.Name.ToLower() == name.ToLower() && a.IsActive).Data;
        }



        public async Task<bool> UpdateAction(RequestUpdateAction request, string userId, CancellationToken cancellationToken)
        {
            #region Request Validations
            if (request == null || string.IsNullOrWhiteSpace(request.Id) || string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Id or Name can't be null");
            }
            var actionResult = await _unitOfWork.action.GetByIdAsync(request.Id, cancellationToken);
            if (actionResult.Data == null)
            {
                throw new RecordNotFoundException("No action found with Id");
            }
            #endregion Request Validations

            actionResult.Data.Name = request.Name;
            actionResult.Data.UpdatedDate = DateTime.Now;
            actionResult.Data.UpdatedBy = userId;

            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }



        public async Task<bool> DeleteAction(string actionId, string userId, CancellationToken cancellationToken)
        {
            if (actionId == null)
            {
                throw new ArgumentNullException("actionId can not be null or empty");
            }

            var getterResult = await _unitOfWork.action.GetSingleAsync(cancellationToken, x => x.Id == actionId && x.IsActive == true);
            if (getterResult.Data == null)
            {
                throw new RecordNotFoundException("No record found with such action ID");
            }

            #region Delete Respective RoleActions
            var roleActionResult = await _unitOfWork.roleAction.GetAsync(cancellationToken, x => x.ActionId == actionId);
            if (roleActionResult.Data.Count() > 0)
            {
                foreach (var roleAction in roleActionResult.Data)
                {
                    roleAction.IsActive = false;
                    roleAction.UpdatedDate = DateTime.UtcNow;
                    roleAction.UpdatedBy = userId;
                }
            }
            #endregion Delete Respective RoleActions

            getterResult.Data.IsActive = false;
            getterResult.Data.UpdatedBy = userId;
            getterResult.Data.UpdatedDate = DateTime.UtcNow;

            //var setterResult = await _unitOfWork.action.UpdateOnConditionAsync(
            //// 1st param: matching condition
            //x => x.IsActive == true && x.Id == actionId,
            //// 2nd param: set the updated value
            //x => x.SetProperty((Func<Actions, string?>)(y => y.Id), (string?)null)
            //      .SetProperty((Func<Actions, string>)(y => y.UpdatedBy), userId)
            //      .SetProperty((Func<Actions, DateTime>)(y => y.UpdatedDate), DateTime.UtcNow)
            //      , cancellationToken);
            //if (setterResult.Result == false)
            //{
            //    throw new InvalidOperationException("Failed to update on conditions provided");
            //}

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }



        public async Task<bool> DeleteActions(string[] actionIds, string userId, CancellationToken cancellationToken)
        {
            if (actionIds == null || actionIds.Length == 0)
            {
                throw new ArgumentException("Action IDs cannot be null or empty.", nameof(actionIds));
            }

            var actionsToDelete = new List<Actions>();

            foreach (var actionId in actionIds)
            {
                var actionResult = await _unitOfWork.action.GetByIdAsync(actionId, cancellationToken);

                if (actionResult?.Data == null)
                {
                    throw new RecordNotFoundException("action with id can't be found" + nameof(actionResult.Data));
                }

                var updatedActions = actionResult.Data;
                updatedActions.IsActive = false;
                updatedActions.UpdatedBy = userId;
                updatedActions.UpdatedDate = DateTime.UtcNow;

                #region Delete Respective RoleActions
                var roleActionResult = await _unitOfWork.roleAction.GetAsync(cancellationToken, x => x.ActionId == actionId);
                if (roleActionResult.Data.Count() > 0)
                {
                    foreach (var roleAction in roleActionResult.Data)
                    {
                        roleAction.IsActive = false;
                        roleAction.UpdatedDate = DateTime.UtcNow;
                        roleAction.UpdatedBy = userId;
                    }
                }
                #endregion Delete Respective RoleActions

                actionsToDelete.Add(updatedActions);
            }

            if (actionsToDelete.Count == 0)
            {
                throw new RecordNotFoundException("No valid actions found to delete.");
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }




        public async Task<List<ResponseGetAllActionDetails>> GetAllAction(string userId, CancellationToken cancellationToken)
        {
            if(userId == null)
            {
                throw new ArgumentNullException("userId"); 
            }

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
                            Id = record.Id,
                            Name = record.Name,
                            Tags = record.Tags
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
                    Name = actions.Name,
                    Tags = actions.Tags
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
            if(existingAction.Data.IsActive == true)
            {
                throw new RecordNotFoundException("Deleted actionId can't be accessed");
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

            var individualTags = actionData.GetActionTag()
                                           .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                           .Select(tag => tag.Trim())
                                           .ToList();

            if (!individualTags.Contains(request.tagToRemove))
            {
                throw new RecordNotFoundException($"Tag '{request.tagToRemove}' does not exist in the action.");
            }

            var updatedTags = string.Join(",", individualTags.Where(tag => tag != request.tagToRemove));

            var updateStatus = await _unitOfWork.action.UpdateOnConditionAsync(
                x => x.IsActive && x.Id == request.actionId,
                x => x.SetProperty(y => y.Tags, updatedTags)
                      .SetProperty(y => y.UpdatedBy, userId)
                      .SetProperty(y => y.UpdatedDate, DateTime.UtcNow),
                cancellationToken);

            if (updateStatus.Result == false)
            {
                throw new InvalidOperationException("Could not remove tags.");
            }

            return updateStatus.Result;
        }




        public async Task<ResponseGetActionsDetailsById> GetActionsDetailsById(string actionId, CancellationToken cancellationToken)
        {
            if(actionId == null)
            {
                throw new ArgumentNullException("actionId can't be null");
            }

            ResponseGetActionsDetailsById response = new ResponseGetActionsDetailsById();

            var existingActionResult = await _unitOfWork.action.GetByIdAsync(actionId, cancellationToken);
            var existingAction = existingActionResult.Data;
            if (existingAction == null)
            {
                throw new RecordNotFoundException(existingActionResult.Message);
            }

            response.Name = existingAction.Name;
            response.Tags = existingAction.Tags;
            return response;
        }



        public async Task<ResponseGetAllFeatures> GetAllFeatures(CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.action.GetAllAsync(cancellationToken);

            if (!result.Status || result.Data == null)
            {
                throw new RecordNotFoundException("Failed to retrieve Actions data.");
            }

            var featureList = result.Data
                .Where(record => record.IsActive && !string.IsNullOrWhiteSpace(record.Name))
                .Select(record =>
                {
                    var parts = record.Name.Split('/');
                    return parts.Length >= 3 ? parts[2] : string.Empty;
                })
                .Where(feature => !string.IsNullOrWhiteSpace(feature))
                .Distinct()
                .ToList();

            return new ResponseGetAllFeatures
            {
                Features = featureList
            };
        }
    }
}
