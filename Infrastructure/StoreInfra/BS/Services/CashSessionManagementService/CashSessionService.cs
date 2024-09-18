using BS.CustomExceptions.Common;
using BS.Services.CashManagementService.Models;
using BS.Services.CashManagementService.Models.Request;
using BS.Services.CashManagementService.Models.Response;
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
            if (entity == null)
            {
                throw new ArgumentException("The request is invalid and could not be converted to a domain entity.", nameof(request));
            }
            entity.Id = Guid.NewGuid().ToString();
            entity.CreatedBy = userId;
            entity.UpdatedBy = userId;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            entity.IsArchived = false;
            entity.IsActive = true;

            if (request.CashDetails != null)
            {
                entity.cashDetails = new List<CashDetails>();

                foreach (var item in request.CashDetails)
                {
                    entity.cashDetails.Add(new CashDetails()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Currency = item.Currency,
                        Type = item.Type,
                        Quantity = item.Quantity,
                        CreatedBy = userId,
                        UpdatedBy = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        IsArchived = false,
                        IsActive = true,
                    });
                }
            }

            await _unitOfWork.CashSessionRepo.AddAsync(entity, userId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            ResponseAddCashSession response = new ResponseAddCashSession();
            response.CashSessionId = entity.Id;

            return response;
        }



        public async Task<ResponseCashSessionDetails> GetCashDetailsByCashSessionId(string CashSessionId, string userId, CancellationToken token)
        {
            if(CashSessionId == null)
            {
                throw new ArgumentNullException("CashSessionId can't be null");
            }

            var response = new ResponseCashSessionDetails();

            var cashSessionResult = await _unitOfWork.CashSessionRepo.GetSingleAsync(token, x => x.Id == CashSessionId, "cashDetails");
            if (cashSessionResult.Data == null)
            {
                throw new RecordNotFoundException("No record exists for such CashSessionId");
            }

            response.TotalAmount = cashSessionResult.Data.TotalAmount;
            response.TillId = cashSessionResult.Data.TillId;
            response.UserId = cashSessionResult.Data.UserId;

            response.CashDetails = new List<CashDetailsResponseObject>();
            if (cashSessionResult.Data.cashDetails != null)
            {
                foreach (var item in cashSessionResult.Data.cashDetails)
                {
                    response.CashDetails.Add(new CashDetailsResponseObject()
                    {
                        Id = item.Id,
                        Currency = item.Currency,
                        Type = item.Type,
                        Quantity = item.Quantity
                    });
                }
            }

            return response;
        }






    }
}
