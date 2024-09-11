using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.PaymentManagementService.Models.Request
{
    public class RequestAddSurchargeDiscount
    {
        public string PaymentType { get; set; }

        public double TotalBill { get; set; }

        public double CashAmount { get; set; }

        public double CardAmount { get; set; }
    }
}
