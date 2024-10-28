using BS.Services.ActionsService.Models.Request;
using BS.Services.NotificationManagementService.Models.Request;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.NotificationManagementService.Models
{
    public static class CRTNotification
    {
        public static Notification ToDomain(this RequestAddNotification request, string userId)
        {
            Notification notification = new Notification();

            notification.Title = request.Title;
            notification.Description = request.Description;
            notification.At = request.At;

            notification.TargetNamespace = request.TargetNamespace;
            notification.UserId = request.UserId;
            notification.Tag = request.Tag;

            notification.CreatedBy = userId;
            notification.UpdatedBy = userId;
            notification.CreatedDate = DateTime.UtcNow;
            notification.UpdatedDate = DateTime.UtcNow;

            return notification;
        }
    }
}
