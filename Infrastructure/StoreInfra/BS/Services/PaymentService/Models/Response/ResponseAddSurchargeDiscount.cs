using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.SurchargeDiscountService.Models.Response
{
    public class ResponseAddSurchargeDiscount
    {
        public double SurchargeAmount { get; set; }

        public double CashDiscountAmount { get; set; }

        public double TotalBillWithSurchargeAndDiscount { get; set; }
    }
}
