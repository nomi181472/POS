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
namespace BS.Services.NotificationManagementService
{
    public interface INotificationManagementService
    {
        Task<bool> AddNotification(Notification b_Tm_Notification, string userId, CancellationToken cancellationToken);

        Task<IEnumerable<ResponseNotificationLogs>> ListAll(CancellationToken cancellationToken);

        Task<IEnumerable<ResponseNotificationLogs>> ListNotificationUserWise(string userId, CancellationToken cancellationToken, int lastCount = 10, int skipRecords = 5);

        Task<bool> UpdateOnClickNotification(string notificationId, string userId, CancellationToken cancellationToken);
    }
}
