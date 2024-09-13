using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.CustomExceptions.Common;
using BS.Services.PaymentMethodService.Models.Response;
using BS.Services.SaleProcessingService.Models.Response;
using DA;
using DM.DomainModels;

namespace BS.Services.PaymentMethodService
{
    public class PaymentMethodsService:IPaymentMethodsService
    {
        private IUnitOfWork _unitOfWork;
        public PaymentMethodsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<PaymentMethodsResponse>> ListAllPaymentMethods(CancellationToken cancellationToken)
        {
            List<PaymentMethodsResponse> response = new List<PaymentMethodsResponse>();
            response = _unitOfWork.PaymentMethodsRepo
                .GetAsync(cancellationToken, x => x.IsActive == true,
                orderBy: q => q.OrderByDescending(p => p.CreatedDate)).Result.Data
                .Select(x => new PaymentMethodsResponse 
                { 
                    Id = x.Id, 
                    Name = x.Name 
                }).ToList();
            if (response == null)
            {
                throw new RecordNotFoundException("No payment methods were found.");
            }
            if (response.Count == 0)
            {
                throw new RecordNotFoundException("No payment methods were found.");
            }
            return response;
        }
    }
}
