﻿using BS.ExternalServices.GrpcClients.Models;
using BS.Services.ActionsService.Models.Request;
using BS.Services.ActionsService.Models.Response;
using BS.Services.RoleService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.ActionsService
{
    public interface IActionService
    {
        bool IsActionsAvailable(string name);
        Task<bool> AddAction(RequestAddAction request, string UserId, CancellationToken cancellationToken);
        Task<bool> AddListOfActions(RequestAddListOfActions request, string UserId, CancellationToken cancellationToken);
        Task<ResponseGetActionsDetailsById> GetActionsDetailsById(string actionId, CancellationToken cancellationToken);
        Task<List<ResponseGetAllActionDetails>> GetAllAction(string userId, CancellationToken CancellationToken);
        Task<ResponseGetAllActionDetails> GetActionById(string actionId, CancellationToken CancellationToken);
        Task<bool> UpdateAction(RequestUpdateAction request, string userId, CancellationToken cancellationToken);
        Task<bool> DeleteAction(string actionId, string userId, CancellationToken CancellationToken);
        Task<bool> DeleteActions(string[] actionIds, string userId, CancellationToken CancellationToken);
        Task<bool> AppendActionTag(RequestAppendActionTag request, string userId, CancellationToken cancellationToken);
        Task<bool> RemoveActionTag(RequestRemoveActionTag request, string userId, CancellationToken cancellationToken);
        Task<ResponseGetAllFeatures> GetAllFeatures(CancellationToken cancellationToken);
    }
}
