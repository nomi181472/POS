using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.Services.PaymentMethodService.Models.Response;

namespace BS.Services.PaymentMethodService
{
    public interface IPaymentMethodsService
    {
        public Task<List<PaymentMethodsResponse>> ListAllPaymentMethods(CancellationToken cancellationToken);
    }
}
