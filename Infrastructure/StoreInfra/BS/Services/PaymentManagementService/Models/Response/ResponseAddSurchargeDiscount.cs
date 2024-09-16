using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.PaymentManagementService.Models.Response
{
    public class ResponseAddSurchargeDiscount
    {
        public double CashDiscountAmount { get; set; }
        public double ChequeSurchargeAmount { get; set; }
        public double OnlineTransferSurchargeAmount { get; set; }
        public double CardSurchargeAmount { get; set; }

        public double CreditAmount { get; set; }
        public double RemainingAmounnt { get; set; }
    }
}
