﻿using BS.Services.CashManagementService.Models.Request;
using BS.Services.OrderService.Models.Request;
using BS.Services.OrderService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.OrderService
{
    public interface IOrderDetailsService
    {
        public Task<ResponseAddOrderDetails> AddOrders(RequestAddOrderDetails request, string userId, CancellationToken token);

        public Task<List<ResponseListOrderDetails>> ListOrderDetailsWithDetails(string userId, CancellationToken token);

        public Task<bool> UpdateOrderDetails(RequestUpdateOrderDetails request, string userId, CancellationToken token);
    }
}
