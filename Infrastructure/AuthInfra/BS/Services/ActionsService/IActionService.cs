﻿using BS.ExternalServices.GrpcClients.Models;
using BS.Services.ActionsService.Models.Request;
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
        Task<bool> AddAction(RequestAddAction request, string UserId, CancellationToken cancellationToken);
        //Task<List<ResponseGetAllAction>> GetAllAction(string userId, CancellationToken CancellationToken);
        //Task<ResponseGetAllUserRoles> GetActionById(string actionId, CancellationToken CancellationToken);
        //Task<bool> DeleteAction(string actionId, string userId, CancellationToken CancellationToken);
        //Task<bool> DeleteActions(string[] actionIds, string userId, CancellationToken CancellationToken);
    }
}
