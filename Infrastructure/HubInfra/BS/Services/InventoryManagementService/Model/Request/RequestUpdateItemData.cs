using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.InventoryManagementService.Model.Request
{
    public class RequestUpdateItemData
    {
        public List<ItemsDetail> Items { get; set; } = new List<ItemsDetail>();
    }

}
