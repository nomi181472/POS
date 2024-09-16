using BS.Services.InventoryManagementService.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.InventoryManagementService.Model.Request
{
    public class RequestAddItemGroup
    {
        public List<ItemGroupDetails> ItemGroups { get; set; }
    }

    public class ItemGroupDetails
    {
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
