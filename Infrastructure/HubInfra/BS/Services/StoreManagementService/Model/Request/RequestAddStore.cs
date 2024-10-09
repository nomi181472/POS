using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.StoreManagementService.Model.Request
{
    public class RequestAddStore
    {
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string LocCode { get; set; }
        public string CompanyCode { get; set; } 
        public string PriceListId { get; set; }
        public string AdminUser { get; set; }
        public string ERPCode { get; set; }
        public string ERPName { get; set; }
    }
}
