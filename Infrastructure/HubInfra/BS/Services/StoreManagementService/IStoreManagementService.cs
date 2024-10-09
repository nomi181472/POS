using BS.Services.StoreManagementService.Model.Request;
using BS.Services.StoreManagementService.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.StoreManagementService
{
    public interface IStoreManagementService
    {
        Task<bool> AddStore(RequestAddStore request,CancellationToken cancellationToken);
        Task<bool> UpdateStore(RequestUpdateStore request,CancellationToken cancellationToken);
        Task<bool> DeleteStore(RequestDeleteStore request,CancellationToken cancellationToken);
        Task<List<ResponseListAllStore>> ListAllStore(CancellationToken cancellationToken);
    }
}
