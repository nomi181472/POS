using BS.Services.InventoryManagementService.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.InventoryManagementService.Model.Response
{
    public class ResponseUpdateItemData
    {
        public List<ItemResponseDetails> ItemResponse { get; set; }
    }
}
