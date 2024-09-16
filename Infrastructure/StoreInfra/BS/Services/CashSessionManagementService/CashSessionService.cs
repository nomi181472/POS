using BS.Services.CashManagementService.Models;
using BS.Services.CashManagementService.Models.Request;
using BS.Services.CashSessionManagementService.Models;
using BS.Services.CashSessionManagementService.Models.Request;
using BS.Services.CashSessionManagementService.Models.Response;
using DA;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CashSessionManagementService
{
    public class CashSessionService : ICashSessionService
    {
        IUnitOfWork _unitOfWork;

        public CashSessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<ResponseAddCashSession> AddCashSession(RequestAddCashSession request, string userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The request cannot be null.");
            }

            var entity = request.ToDomain();

            entity.Id = Guid.NewGuid().ToString();
            entity.CreatedBy = userId;
            entity.UpdatedBy = userId;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            entity.IsArchived = false;
            entity.IsActive = true;

            foreach (var item in request.CashDetails)
            {
                entity.cashDetails = new List<CashDetails>()
                {
                    new CashDetails()
                    {
                        Id = Guid.NewGuid().ToString(),

                        Currency = item.Currency,
                        Type = item.Type,
                        Quantity = item.Quantity,

                        CashSessionId = entity.Id,

                        CreatedBy = userId,
                        UpdatedBy = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        IsArchived = false,
                        IsActive = true,
                    }
                };
            }

            if (entity == null)
            {
                throw new ArgumentException("The request is invalid and could not be converted to a domain entity.", nameof(request));
            }

            await _unitOfWork.CashSessionRepo.AddAsync(entity, userId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            ResponseAddCashSession response = new ResponseAddCashSession();
            response.CashSessionId = entity.Id;

            return response;
        }
    }
}
