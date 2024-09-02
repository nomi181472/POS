using BS.Services.CashManagementService.Models;
using BS.Services.CashManagementService.Models.Request;
using BS.Services.CashManagementService.Models.Response;
using DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BS.Services.CashManagementService
{
    public class CashManagementService : ICashManagementService
    {
        IUnitOfWork _unitOfWork;

        public CashManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddCash(RequestAddCash request, string userId, CancellationToken cancellationToken)
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
            entity.UpdatedBy = userId;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            entity.IsArchived = false;
            entity.IsActive = true;

            await _unitOfWork.CashManagementRepo.AddAsync(entity, userId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }

        public async Task<List<ResponseListCash>> ListCashWithDetails(string userId, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.CashManagementRepo.GetAllAsync(cancellationToken);
            List<ResponseListCash> response = new List<ResponseListCash>();

            if (result.Status)
            {
                foreach (var record in result.Data)
                {
                    if (record.IsActive == true)
                    {
                        response.Add(new ResponseListCash()
                        {
                            Currency = record.Currency,
                            Type = record.Type,
                            Count = record.Count
                        });
                    }
                }
                return response;
            }
            else
            {
                throw new InvalidOperationException("Failed to retrieve cash data.");
            }
        }


    }
}
