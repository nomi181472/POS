using BS.Services.PaymentManagementService.Models.Request;
using BS.Services.PaymentManagementService.Models.Response;
using BS.Services.SaleProcessingService.Models.Request;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.PaymentManagementService
{
    public interface IPaymentManagementService
    {
        Task<ResponseAddSurchargeDiscount> AddSurchargeDiscount(CreateOrderRequest createOrderRequest, Order order, string userId, CancellationToken token);
    }
}
