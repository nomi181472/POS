using Grpc.Core;
using AuthService;
using BS.Services.NotificationManagementService;
using Auth.Middlewares;
using BS.Services.NotificationManagementService.Models.Request;

namespace Auth.AuthGrpc
{
    public class NotificationGrpcImpl : AuthServiceGrpc.AuthServiceGrpcBase
    {
        INotificationManagementService _notificationManagementService;
        IUserContext _userContext;

        public NotificationGrpcImpl(INotificationManagementService notificationManagementService, IUserContext userContext)
        {
            _notificationManagementService = notificationManagementService;
            _userContext = userContext;
        }

        public override async Task<NotificationAddResponse> SendNotification(NotificationAddRequest grpcRequest, ServerCallContext context)
        {
            DateTime time = DateTime.UtcNow;
            if(!DateTime.TryParse(grpcRequest.At,out time))
            {
                throw new ArgumentException("Incorrect date time");
            }

            RequestAddNotification restRequest = new RequestAddNotification()
            {
                Title = grpcRequest.Title,
                Message = grpcRequest.Message,
                Description = grpcRequest.Description,
                At = time,
                TargetNamespace = grpcRequest.TargetNamespace,
                SendToUserType = grpcRequest.SendToUserType,
                Tag = grpcRequest.SendToUserType,
                UserId = grpcRequest.UserId
            };

            var restResponse = await _notificationManagementService.AddNotification(restRequest, _userContext.Data.UserId, context.CancellationToken);

            NotificationAddResponse grpcResponse = new NotificationAddResponse()
            {
                Message = restResponse
            };

            return grpcResponse;
        }
    }
}
