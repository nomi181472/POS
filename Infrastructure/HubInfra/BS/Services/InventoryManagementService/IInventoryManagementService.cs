using BS.Services.AreaCoverageManagementService.Model.Response;
using BS.Services.InventoryManagementService.Model.Request;
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
    }
}
