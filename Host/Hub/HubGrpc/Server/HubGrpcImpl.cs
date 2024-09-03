using HubService;
using Grpc.Core;


namespace Hub.HubGrpc.Server
{
    public class HubGrpcImpl : HubServiceGRPC.HubServiceGRPCBase
    {
        public override async Task<DataResponse> SendData(DataRequest request, ServerCallContext context)
        {
            //Console.WriteLine($"Received data: {request.Data}");

            return new DataResponse
            {
                Message = $"{request.Data}"
            };
        }

        public override async Task<CustomerAddResponse> AddCustomer(CustomerAddRequest request, ServerCallContext context)
        {
            return new CustomerAddResponse()
            {
                Message =  "True",
                Id = request.Id,
                Name = "Good",
            };
        }

    }
}
