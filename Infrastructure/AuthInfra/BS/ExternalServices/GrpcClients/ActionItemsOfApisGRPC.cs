using ActionExposedGRPC;
using BS.ExternalServices.GrpcClients.Models;
using Grpc.Core;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BS.ExternalServices.GrpcClients;



public static class ActionItemsOfApisGRPC
{
   
    public static async Task<Tuple<string,bool,ResponseGetAllActions?>> GetEndpoints(string host, int port, string servername, DateTime deadline, CancellationToken token)
    {
        string message = "";
        try
        {
            
            string url = $"{host}:{port}";

            Channel channel = new Channel(url, ChannelCredentials.Insecure)
            {

            };
            await channel.ConnectAsync();

            //TODO: log Console.WriteLine("Connected to server");
            var client = new ActionExposedServiceGRPC.ActionExposedServiceGRPCClient(channel);
            var result = await client.GetActionsAsync(new RequestGetActions(), cancellationToken: token);
            if(result.IsApiHandled && result.IsRequestSuccess)
            {
                var data = new ResponseGetAllActions()
                {
                   ApiName= result.Data.ApiName,Routes= result.Data.Routes.ToList()
                };
                var response = Tuple.Create<string,bool, ResponseGetAllActions?> (result.Message, true, data);
                return response;
            }
           
            message = result.Message + string.Join(",", result.Exceptions.ToArray());
            return Tuple.Create<string, bool, ResponseGetAllActions?>(message, true, new ResponseGetAllActions()
            {
                ApiName= result.Data.ApiName,
                IsWorking=false,
                Message=message,
                Routes= result.Data.Routes.ToList()
            });
            
        }
        catch (Exception ex)
        {
            //TODO:Logging
            message = ex.Message;
           
        }


        return Tuple.Create<string, bool, ResponseGetAllActions?>(message, false, new ResponseGetAllActions()
        {
            ApiName = servername,
            IsWorking = false,
            Message = message,
            Routes =new List<string>(),
        });
       


    }
}
