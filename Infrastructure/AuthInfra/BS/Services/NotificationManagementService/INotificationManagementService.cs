using BS.Services.NotificationManagementService.Models.Response;
using DA;
using DM.DomainModels;
using Helpers.CustomExceptionThrower;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BS.Services.NotificationManagementService.Models.Request;
namespace BS.Services.NotificationManagementService
{
    public interface INotificationManagementService
    {
        Task<bool> AddNotification(RequestAddNotification notification, string userId, CancellationToken cancellationToken);

        Task<List<ResponseNotificationLogs>> ListAll(CancellationToken cancellationToken);

        Task<List<ResponseNotificationLogs>> ListNotificationUserWise(string userId, CancellationToken cancellationToken, int lastCount = 10, int skipRecords = 5);

        Task<bool> UpdateOnClickNotification(RequestUpdateOnClickNotification request, string userId, CancellationToken cancellationToken);
    }
}
