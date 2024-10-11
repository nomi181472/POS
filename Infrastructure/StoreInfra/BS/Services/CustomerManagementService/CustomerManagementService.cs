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
using System.Threading;
using BS.Services.CashManagementService.Models.Response;
using BS.Services.CustomerManagementService.Models.Response;
using BS.CustomExceptions.Common;
using DM.DomainModels;

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



        public async Task<ResponseAddCustomer> AddCustomer(RequestAddCustomer request, string userId, CancellationToken cancellationToken)
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

            await _unitOfWork.CustomerManagementRepo.AddAsync(entity, userId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);


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
                    Id = entity.Id,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    Cnic = request.Cnic,
                    Billing = request.Billing,
                    Address = request.Address
                };

                var verify = await client.AddCustomerAsync(GRPCrequest, cancellationToken: cancellationToken);

                ResponseAddCustomer response = new ResponseAddCustomer();
                response.Status = true;
                response.CustomerId = entity.Id;
                return response;
            }
            catch
            {
                throw new Exception("An error occured for grpc but till happened");
            }
        }



        public async Task<bool> UpdateCustomer(RequestUpdateCustomer request, string userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The request cannot be null.");
            }

            var setterResult = await _unitOfWork.CustomerManagementRepo.UpdateOnConditionAsync(
                // 1st param: matching condition
                x => x.IsActive == true && x.Id == request.Id,
                // 2nd param: set the updated value
                x => x.SetProperty(x => x.Name, request.Name)
                .SetProperty(x => x.PhoneNumber, request.PhoneNumber)
                .SetProperty(x => x.Email, request.Email)
                .SetProperty(x => x.UpdatedBy, userId)
                .SetProperty(x => x.UpdatedDate, DateTime.UtcNow)
                , cancellationToken
            );


            if (setterResult == null)
            {
                throw new InvalidOperationException("The update operation did not return a result.");
            }

            await _unitOfWork.CommitAsync(cancellationToken);

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

                var GRPCrequest = new CustomerUpdateRequest()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    Cnic = request.Cnic,
                    Billing = request.Billing,
                    Address = request.Address
                };

                var verify = await client.UpdateCustomerAsync(GRPCrequest, cancellationToken: cancellationToken);

                return true;
            }
            catch
            {
                throw new Exception("An error occured for grpc but till happened");
            }


        }


        public async Task<List<ResponseListCustomerWithDetails>> ListCustomerWithDetails(string userId, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.CustomerManagementRepo.GetAllAsync(cancellationToken);
            List<ResponseListCustomerWithDetails> response = new List<ResponseListCustomerWithDetails>();

            if (result.Status)
            {
                foreach (var record in result.Data)
                {
                    if (record.IsActive == true)
                    {
                        response.Add(new ResponseListCustomerWithDetails()
                        {
                            CustomerId = record.Id,
                            Name = record.Name,
                            PhoneNumber = record.PhoneNumber,
                            Email = record.Email,
                            Cnic = record.Cnic,
                            Billing = record.Billing,
                            Address = record.Address
                        });
                    }
                }
                return response;
            }
            else
            {
                throw new InvalidOperationException("Failed to retrieve customer data.");
            }
        }



        public async Task<ResponseGetCustomerHistoryById> GetCustomerHistoryById(string customerId, CancellationToken cancellationToken)
        {
            /// SELECT ItemId AS itemid, Quantity FROM CustomerCartItems WHERE CartId = (SELECT Id FROM CustomerCart WHERE CustomerId = request.CustomerId AND IsConvertedToSale)
            /// SELECT ItemName, ItemGroupCode FROM Items WHERE Id = itemid
            /// SELECT GroupCodeName FROM ItemGroup WHERE Code = ItemGroupCode

            ResponseGetCustomerHistoryById response = new ResponseGetCustomerHistoryById();

            #region Validations
            if (customerId == null)
            {
                throw new ArgumentException("No record can exist for null customer ID");
            }
            var existingCustomerResult = await _unitOfWork.CustomerManagementRepo.GetByIdAsync(customerId, cancellationToken);
            if (existingCustomerResult.Data == null)
            {
                throw new RecordNotFoundException("Customer ID not found");
            }
            var customerCartResults = await _unitOfWork.CustomerCartRepo.GetAsync(cancellationToken, x => x.CustomerId == customerId && x.IsConvertedToSale);
            if (customerCartResults.Data == null || !customerCartResults.Data.Any())
            {
                throw new RecordNotFoundException("No cart for customer ID found");
            }
            #endregion Validations

            #region Query
            var customerHistoryList = new List<CustomerHistoryDTO>();
            foreach (var customerCartResult in customerCartResults.Data)
            {
                var customerCartItemsResults = await _unitOfWork.CustomerCartItemsRepo.GetAsync(cancellationToken, x => x.Id == customerCartResult.Id);
                if (customerCartItemsResults.Data == null) { throw new RecordNotFoundException("No Cart Items Found for customer ID"); }
                foreach (var customerCartItemsResult in customerCartItemsResults.Data)
                {
                    var itemResult = await _unitOfWork.ItemsRepo.GetByIdAsync(customerCartItemsResult.ItemId, cancellationToken);
                    if (itemResult.Data == null) { throw new RecordNotFoundException("No items found for the cart"); }
                    var itemGroupResult = await _unitOfWork.ItemGroupRepo.GetByIdAsync(itemResult.Data.ItemGroupId, cancellationToken);

                    customerHistoryList.Add(new CustomerHistoryDTO
                    {
                        ItemName = itemResult.Data.ItemName,
                        Quantity = customerCartItemsResult.Quantity,
                        GroupCodeName = itemGroupResult.Data.Name
                    });
                }
            }
            #endregion Query

            response.CustomerName = existingCustomerResult.Data.Name;
            response.CustomerHistory = customerHistoryList;
            return response;
        }

    }
}
