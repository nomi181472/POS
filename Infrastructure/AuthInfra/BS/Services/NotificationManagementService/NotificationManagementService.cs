﻿using BS.CustomExceptions.Common;
using BS.Services.NotificationManagementService.Models;
using BS.Services.NotificationManagementService.Models.Request;
using BS.Services.NotificationManagementService.Models.Response;
using DA;
using DM.DomainModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.NotificationManagementService
{
    public class NotificationManagementService : INotificationManagementService
    {
        private IUnitOfWork _uow;
        public NotificationManagementService(IUnitOfWork uot)
        {
            _uow = uot;
        }



        public async Task<bool> AddNotification(RequestAddNotification request, string userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentException("Notification can't be null");
            }

            var existingUser = await _uow.user.GetByIdAsync(request.UserId, cancellationToken);
            if (existingUser.Data == null)
            {
                throw new RecordNotFoundException("no user with userId found");
            }

            var entity = request.ToDomain(userId);
            await _uow.notification.AddAsync(entity, userId, cancellationToken);
            await _uow.CommitAsync(cancellationToken);
            return true;
        }



        public async Task<List<ResponseNotificationLogs>> ListAll(CancellationToken cancellationToken)
        {
            List<ResponseNotificationLogs> response = new List<ResponseNotificationLogs>();
            var result = await _uow.notification.GetAsync(cancellationToken, includeProperties:$"IsSeen");
            if(result.Data == null || result.Data.Count() == 0)
            {
                throw new RecordNotFoundException("no notification records found");
            }

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



        public async Task<List<ResponseNotificationLogs>> ListNotificationUserWise(string userId, CancellationToken cancellationToken, int lastCount, int skipRecords)
        {
            List<ResponseNotificationLogs> response = new List<ResponseNotificationLogs>();
            var query = _uow.notification.GetQueryable();
            ArgumentFalseException.ThrowIfFalse(query.Status, query.Message);

            var result = await query.Data
                                        .Where(x => x.UserId == userId)
                                        .OrderByDescending(x => x.At).Skip(skipRecords).Take(lastCount)
                                        .Include(x => x.IsSeen)
                                        .ToListAsync(cancellationToken);

            if (result == null)
            {
                throw new RecordNotFoundException("No notification records found");
            }

            response = result.Select(x => new ResponseNotificationLogs()
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



        public async Task<bool> UpdateOnClickNotification(RequestUpdateOnClickNotification request, string userId, CancellationToken cancellationToken)
        {
            var result = await _uow.notificationSeen.GetByIdAsync(request.NotificationId, cancellationToken);
            ArgumentFalseException.ThrowIfFalse(result.Status, result.Message);

            if (result.Data == null)
            {
                throw new RecordNotFoundException("No record found");
            }

            NotificationSeen notificationSeen = new NotificationSeen();
            notificationSeen.Id = request.NotificationId;
            notificationSeen.OnClickDate = DateTime.UtcNow;
            notificationSeen.By = userId;

            await _uow.notificationSeen.AddAsync(notificationSeen, userId, cancellationToken);
            await _uow.CommitAsync();
            return true;
        }



    }
}
