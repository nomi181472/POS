using BS.Services.InventoryManagementService.Models.Response;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.InventoryManagementService
{
    public interface IInventoryManagementService
    {
        public Task<List<ResponseGetInventory>> GetInventoryData(string filter, CancellationToken cancellationToken);
        public Task<ResponseReloadInventory> ReloadInventory(CancellationToken cancellationToken);
    }
}
