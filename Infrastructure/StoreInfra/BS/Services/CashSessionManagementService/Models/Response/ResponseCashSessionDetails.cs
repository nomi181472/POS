using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CashSessionManagementService.Models.Response
{
    public class ResponseCashSessionDetails
    {
        public double TotalAmount { get; set; }
        public string TillId { get; set; }
        public string UserId { get; set; }
        public List<CashDetailsResponseObject> CashDetails { get; set; } = new List<CashDetailsResponseObject> { };

    }

    public class CashDetailsResponseObject
    {
        public string Id { get; set; }
        public string Currency { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
    }
}
