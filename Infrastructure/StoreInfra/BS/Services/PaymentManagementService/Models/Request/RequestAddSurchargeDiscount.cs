using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.PaymentManagementService.Models.Request
{
    public class RequestAddSurchargeDiscount
    {
        //public string PaymentType { get; set; }

        public double TotalBill { get; set; }

        public string DiscountVoucherCode { get; set; }
        public int LoyaltyPointsRedeemed { get; set; }
        public double ChequeAmount { get; set; }
        public double OnlineTransferAmount {  get; set; }
        public double CashAmount { get; set; }
        public double CardAmount { get; set; }
        public bool IsCredit { get; set; }
    }
}
