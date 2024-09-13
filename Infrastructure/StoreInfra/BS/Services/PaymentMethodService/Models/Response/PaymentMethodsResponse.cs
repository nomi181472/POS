using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.PaymentMethodService.Models.Response
{
    public class PaymentMethodsResponse
    {
        public virtual string? Id {  get; set; }
        public virtual string? Name { get; set; }
    }
}
