using ActionExposedGRPC;
using BS.ExternalServices.GrpcClients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BS.ExternalServices.GrpcClients
{
    public interface IActionControllerService
    {
        Task<List<ResponseGetAllActions>> GetAllActionsOfAllApis(string ApiBaseName, Assembly assembly, Type iFeature,CancellationToken token);
    }
}
