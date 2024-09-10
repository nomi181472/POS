using BS.Services.SurchargeDiscountService.Models.Request;
using BS.Services.SurchargeDiscountService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.SurchargeDiscountService.Models
{
    public interface IPaymentService
    {
        Task<ResponseAddSurchargeDiscount> AddSurchargeDiscount(RequestAddSurchargeDiscount request, string userId, CancellationToken token);
    }
}
