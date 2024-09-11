using BS.Services.PaymentManagementService.Models.Request;
using BS.Services.PaymentManagementService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.PaymentManagementService
{
    public interface IPaymentManagementService
    {
        Task<ResponseAddSurchargeDiscount> AddSurchargeDiscount(RequestAddSurchargeDiscount request, string userId, CancellationToken token);

        Task<ResponseAddSplitPayments> AddSplitPayments(RequestAddSplitPayments request, string userId, CancellationToken token);
    }
}
