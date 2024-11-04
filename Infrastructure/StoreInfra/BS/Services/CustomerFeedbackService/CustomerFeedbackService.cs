using AuthService;
using BS.Services.CustomerFeedbackService.Models.Request;
using BS.Services.CustomerFeedbackService.Models.Response;
using Grpc.Net.Client;
using HubService;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CustomerFeedbackService
{
    public class CustomerFeedbackService : ICustomerFeedbackService
    {
        IConfiguration _configuration;

        public CustomerFeedbackService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseAddCustomerFeedback> AddCustomerFeedback(RequestAddCustomerFeedback request, string userId, CancellationToken token)
        {
            ResponseAddCustomerFeedback response = new ResponseAddCustomerFeedback();

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // save to DB
            // send to Hub using gRPC

            /// Send Notification to Auth

            try
            {
                string? port = _configuration.GetSection("Auth:Port").Value;
                string? host = _configuration.GetSection("Auth:Host").Value;
                string url = "";
                if (String.IsNullOrEmpty(port) || String.IsNullOrEmpty(host))
                {
                    throw new Exception("GRPC Port and Host is required.");
                }
                else
                {
                    url = $"{host}:{port}";
                }
                var channel = GrpcChannel.ForAddress(url);
                var client = new AuthServiceGrpc.AuthServiceGrpcClient(channel);

                var GRPCrequest = new NotificationAddRequest()
                {
                    Title = "grpcRequest.Title",
                    Message = "grpcRequest.Message",
                    Description = "grpcRequest.Description",
                    At = "2024-10-31T08:01:37.328Z",
                    TargetNamespace = "grpcRequest.TargetNamespace",
                    SendToUserType = "grpcRequest.SendToUserType",
                    Tag = "grpcRequest.SendToUserType",
                    UserId = "67740b1a-94dc-4b36-a6f2-fea8ba111852"
                };

                var grpcResponse = client.SendNotification(GRPCrequest, cancellationToken: token);

                response.Message = grpcResponse.Message;
                
                return response;
            }
            catch
            {
                throw new Exception("An error occured for grpc but till happened");
            }
        }
    }
}
