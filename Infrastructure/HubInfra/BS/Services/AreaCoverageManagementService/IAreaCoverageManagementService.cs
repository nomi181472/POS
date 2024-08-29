using BS.Services.AreaCoverageManagementService.Model.Request;
using BS.Services.AreaCoverageManagementService.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.AreaCoverageManagementService
{
    public interface IAreaCoverageManagementService
    {
        Task<ResponseAddArea> AddArea(RequestAddArea request, string userId, CancellationToken token);
    }
}
