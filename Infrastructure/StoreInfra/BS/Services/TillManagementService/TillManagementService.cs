﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.DomainModels;
using BS.Services.TillManagementService.Request;
using DA;

namespace BS.Services.TillManagementService
{
    public class TillManagementService: ITillManagementService
    {
        IUnitOfWork _unitOfWork;

        public TillManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AddTill(RequestAddTill request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The request cannot be null.");
            }

            Till till = new Till();
            till.Id = Guid.NewGuid().ToString();
            till.Name = request.Name;
            till.Description = request.Description;
            till.IsActive = true;
            till.IsArchived = false;

            await _unitOfWork.TillRepo.AddAsync(till, request.CreatedBy ?? "", cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return true; 
        }
    }
}
