using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CashManagementService.Models.Response
{
    public class ResponseGetCashDetailByCashSessionId
    {
        public double TotalAmount { get; set; }
        public string TillId { get; set; }
        public string UserId { get; set; } 
        public List<CashDetailsResponse> CashDetails { get; set; } = new List<CashDetailsResponse> { };

    }

    public class CashDetailsResponse
    {
        public string Id { get; set; }  
        public string Currency { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
    }
}
