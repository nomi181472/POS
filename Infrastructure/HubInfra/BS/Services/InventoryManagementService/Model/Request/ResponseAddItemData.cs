using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.InventoryManagementService.Model.Request
{
    public class ResponseAddItemData
    {
        public List<ItemResponseDetails> ItemResponse { get; set; }
    }

    public class ItemResponseDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Status { get; set;}
    }
}
