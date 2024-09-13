using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.Services.TillManagementService.Request;
using BS.Services.TillManagementService.Response;

namespace BS.Services.TillManagementService
{
    public interface ITillManagementService
    {
        public Task<bool> AddTill(RequestAddTill request, CancellationToken cancellationToken);
        public Task<List<ListAllTillsResponse>> ListAllTills(CancellationToken cancellationToken);
    }
}
