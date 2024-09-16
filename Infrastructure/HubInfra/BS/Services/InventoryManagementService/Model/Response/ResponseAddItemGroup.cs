using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.InventoryManagementService.Model.Response
{
    public class ResponseAddItemGroup
    {
        public List<ItemGrpResponseDetails> ItemGrpsResponse { get; set; }
    }

    public class ItemGrpResponseDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public bool IsSuccess { get; set; }
        public string Status { get; set; }
    }


}
