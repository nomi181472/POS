using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CashManagementService.Models.Request
{
    public class RequestAddCash
    {
        public string Currency {  get; set; }

        public string Type { get; set; }

        public int Count { get; set; }
    }
}
