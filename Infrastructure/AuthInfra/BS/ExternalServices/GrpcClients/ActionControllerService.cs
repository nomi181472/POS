using ActionExposedGRPC;
using BS.ExternalServices.GrpcClients.Models;
using EndpointGRPC.Exposed;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BS.ExternalServices.GrpcClients
{
    public class ActionControllerService : IActionControllerService
    {
     

        public async Task<List<ResponseGetAllActions>> GetAllActionsOfAllApis(string ApiBaseName, Assembly assembly, Type iFeature, CancellationToken token)
        {
            List<ResponseGetAllActions> response = new List<ResponseGetAllActions>();
            var apiEndpoints = ExposedAllEndpoints.GetAllActions(ApiBaseName, assembly, iFeature);
            response.Add(new ResponseGetAllActions()
            {
                ApiName = ApiBaseName,
                Routes = apiEndpoints,
                IsWorking = true,
                Message="",
            });
            var timeout = TimeSpan.FromSeconds(5); // Timeout duration
            var timoutInSec = 12;
            List<Task<Tuple<string, bool, ResponseGetAllActions>>> tasks = new List<Task<Tuple<string, bool, ResponseGetAllActions>>>();

            tasks.Add(AsyncTimeoutHelper.RunWithTimeout(
                ct => ActionItemsOfApisGRPC.GetEndpoints(ApiConfigs.StoreApiConfig.Host, ApiConfigs.StoreApiConfig.Port, ApiConfigs.StoreApiConfig.Name, DateTime.UtcNow.AddSeconds(timoutInSec), ct),
                timeout,
                Tuple.Create<string, bool, ResponseGetAllActions>($"Time Expired of {ApiConfigs.StoreApiConfig.Name} api.", false, new ResponseGetAllActions(){ ApiName= ApiConfigs.StoreApiConfig.Name ,Routes=new List<string>()}),
                token));

            tasks.Add(AsyncTimeoutHelper.RunWithTimeout(
                ct => ActionItemsOfApisGRPC.GetEndpoints(ApiConfigs.HubApiConfig.Host, ApiConfigs.HubApiConfig.Port, ApiConfigs.HubApiConfig.Name, DateTime.UtcNow.AddSeconds(timoutInSec), ct),
                timeout,
                Tuple.Create<string, bool, ResponseGetAllActions>($"Time Expired of {ApiConfigs.HubApiConfig.Name} api.", false, new ResponseGetAllActions() { ApiName = ApiConfigs.HubApiConfig.Name, Routes = new List<string>() }),
                token));

            var results = await Task.WhenAll(tasks); // Wait for all tasks to complete

            foreach (var result in results)
            {
                if (result.Item2)
                {
                    response.Add(new ResponseGetAllActions
                    {
                        ApiName = result.Item3.ApiName,
                        Routes = result.Item3.Routes,
                        IsWorking = result.Item2,
                        Message = result.Item1,

                    });
                }
                else
                {
                    response.Add(new ResponseGetAllActions
                    {
                        ApiName = result.Item3.ApiName,
                        Routes = result.Item3.Routes,
                        IsWorking = result.Item2,
                        Message = result.Item1,

                    });
                }
               
                
            }
            

            return response;
        }


    }
}
