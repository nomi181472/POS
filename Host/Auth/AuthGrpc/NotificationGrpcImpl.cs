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
            RequestAddNotification restRequest = new RequestAddNotification()
            {
                Title = grpcRequest.Title,
                Message = grpcRequest.Message,
                Description = grpcRequest.Description,
                At = DateTime.Parse(grpcRequest.At),
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
