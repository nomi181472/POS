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
    public class NotificationManagementService : INotificationManagementService
    {

        private IUnitOfWork _uow;
         
        public NotificationManagementService(IUnitOfWork uot)
        {
            _uow = uot;

        }

        public async Task<bool> AddNotification(Notification b_Tm_Notification,string userId, CancellationToken cancellationToken)
        {
            bool response = false;
            _uow.Notification.Add(b_Tm_Notification,userId);
            await _uow.CommitAsync(cancellationToken);
           
            return response;
        }

        public async Task<IEnumerable<ResponseNotificationLogs>> ListAll(CancellationToken cancellationToken)
        {
            List<ResponseNotificationLogs> response = new List<ResponseNotificationLogs>();
            var result = _uow.Notification.Get(x=>x.IsActive,null,includeProperties: $"{nameof(NotificationSeen)}");
            ArgumentFalseException.ThrowIfFalse(result.Status, result.Message);
           
            response = result.Data.Select(x => new ResponseNotificationLogs()
            {
                SendToUserType = x.SendToUserType,
                At = x.At,
                Description = x.Description,
                IsOnClick = x.IsSeen != null,
                Message = x.Message,
                OnClickDate = x.IsSeen != null ? x.IsSeen.OnClickDate.ToString() : "NA",
                NotificationId = x.Id,
                Tag = x.Tag,
                TargetNamespace = x.TargetNamespace,
                Title = x.Title,
               

            }).ToList();

            

            return response;
        }

        public async Task<IEnumerable<ResponseNotificationLogs>> ListNotificationUserWise(string userId, CancellationToken cancellationToken, int lastCount, int skipRecords)
        {
            List<ResponseNotificationLogs> response = new List<ResponseNotificationLogs>();
            var query = _uow.Notification.GetQueryable();
            ArgumentFalseException.ThrowIfFalse(query.Status, query.Message);

            var result = await query.Data
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.At).Skip(skipRecords).Take(lastCount)
                .Include(x => x.IsSeen)
                .ToListAsync(cancellationToken);

            if (result == null)
            {
               
                return response;
            }
            response = result.Select(x => new ResponseNotificationLogs()
            {
               
                SendToUserType = x.SendToUserType,
                At = x.At,
                Description = x.Description,
                IsOnClick = x.IsSeen != null,
                Message = x.Message,
                OnClickDate = x.IsSeen != null ? x.IsSeen.OnClickDate.ToString() : "NA",
              NotificationId=x.Id,
                Tag = x.Tag,
                TargetNamespace = x.TargetNamespace,
                Title = x.Title,
                

            }).ToList();

            

            return response;
        }

        public async Task<bool> UpdateOnClickNotification(string notificationId, string userId, CancellationToken cancellationToken)
        {
            var result = await _uow.NotificationSeen.Any(x => x.NotificationId == notificationId);
            ArgumentFalseException.ThrowIfFalse(result.Status,result.Message);
            bool response = false;
            if (result.Data)
            {
                return response;
            }
            _uow.NotificationSeen.Add(new NotificationSeen()
            {
                NotificationId = notificationId,
                OnClickDate = DateTime.UtcNow,
                By = userId,
                

            }, userId);
            await _uow.CommitAsync();
           
            return response;
        }
    }
}
