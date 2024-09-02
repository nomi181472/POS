using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CashManagementService.Models.Response
{
    public class ResponseListCash
    {
        public string Currency { get; set; }

        public string Type { get; set; }

        public int Count { get; set; }
    }
}
