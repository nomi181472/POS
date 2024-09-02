﻿using BS.Services.OrderService.Models;
using BS.Services.OrderService.Models.Request;
using BS.Services.OrderService.Models.Response;
using DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BS.Services.OrderService
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseAddOrderDetails> AddOrders(RequestAddOrderDetails request, string userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The request cannot be null.");
            }

            var entity = request.ToDomain();

            if (entity == null)
            {
                throw new ArgumentException("The request is invalid and could not be converted to a domain entity.", nameof(request));
            }

            entity.Id = Guid.NewGuid().ToString();
            entity.UpdatedBy = userId;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            entity.IsArchived = false;
            entity.IsActive = true;

            await _unitOfWork.OrderDetailsRepo.AddAsync(entity, userId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            ResponseAddOrderDetails response = new ResponseAddOrderDetails()
            {
                isSuccess = true,
            };
            return response;
        }
    }
}