using BS.Services.CashManagementService.Models;
using BS.Services.CashManagementService.Models.Request;
using BS.Services.CustomerManagementService.Models;
using BS.Services.CustomerManagementService.Models.Request;
using DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubService;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace BS.Services.CustomerManagementService
{
    public class CustomerManagementService : ICustomerManagementService
    {
        IUnitOfWork _unitOfWork;
        IConfiguration _configuration;

        public CustomerManagementService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<bool> AddCustomer(RequestAddCustomer request, string userId, CancellationToken cancellationToken)
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

            //await _unitOfWork.CustomerManagementRepo.AddAsync(entity, userId, cancellationToken);
            //await _unitOfWork.CommitAsync(cancellationToken);


            try
            {
                string? port = _configuration.GetSection("Hub:Port").Value;
                string? host = _configuration.GetSection("Hub:Host").Value;

                string url = "";
                if (String.IsNullOrEmpty(port) || String.IsNullOrEmpty(host))
                {
                    throw new Exception("GRPC Port and Host is required.");
                }
                else
                {
                    url = $"{host}:{port}";
                }

                var channel = GrpcChannel.ForAddress(url);
                var client = new HubServiceGRPC.HubServiceGRPCClient(channel);
                var GRPCrequest = new CustomerAddRequest()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    Cnic = request.Cnic,
                    Billing = request.Billing,
                    Address = request.Address

                };


                var verify = await client.AddCustomerAsync(GRPCrequest, cancellationToken: cancellationToken);

                return true;
            }
            catch
            {
                throw new Exception("An error occured for grpc but till happened");
            }

            
        }
    }
}
