using BS.CustomExceptions.Common;
using BS.Services.ActionsService.Models;
using BS.Services.ActionsService.Models.Request;
using DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.ActionsService
{
    public class ActionService : IActionService
    {
        IUnitOfWork _unitOfWork;
        public ActionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            return true;
        }




    }
}
