using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Store : Base<string>
    {
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string StorageLocationId { get; set; }
        public string PriceListId { get; set; }
        public string AdminUser { get; set; }

        public StorageLocation StorageLocation { get; set; }
    }
}
