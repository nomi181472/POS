using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CashSessionManagementService.Models.Request
{
    public class RequestAddCashSession
    {
        public string TillId {  get; set; }
        public string UserId { get; set; }
        public double TotalAmount { get; set; }

        public List<CashDetailsObject>? CashDetails { get; set; }
    }

    public class CashDetailsObject
    {
        public string Type { get; set; }
        public string Currency { get; set; }
        public int Quantity { get; set; }

    }
}
