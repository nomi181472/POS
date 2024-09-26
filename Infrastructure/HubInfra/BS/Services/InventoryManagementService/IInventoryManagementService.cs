using BS.Services.AreaCoverageManagementService.Model.Response;
using BS.Services.InventoryManagementService.Model.Request;
using BS.Services.InventoryManagementService.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.InventoryManagementService
{
    public interface IInventoryManagementService
    {
        Task<ResponseAddItemData> AddItemData(RequestAddItemData request, string userId, CancellationToken token);
        Task<ResponseUpdateItemData> UpdateItemData(RequestUpdateItemData request, string userId, CancellationToken token);
        //Task<ResponseAddItemGroup> AddItemGroup(RequestAddItemGroup request, string userId, CancellationToken token);
        public bool IsItemExist(string Code);
        //public bool IsItemGroupExist(int Code);
    }
}
