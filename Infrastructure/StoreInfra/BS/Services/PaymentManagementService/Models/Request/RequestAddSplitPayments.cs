using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.PaymentManagementService.Models.Request
{
    public class RequestAddSplitPayments
    {
        public double TotalBill { get; set; }
        public double CashPayment { get; set; }
        public double CardPayment { get; set; }
    }
}
