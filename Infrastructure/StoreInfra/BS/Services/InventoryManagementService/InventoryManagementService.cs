using DA;
using Grpc.Net.Client;
using Grpc.Core;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubService;
using System.Reflection.PortableExecutable;
using BS.Services.InventoryManagementService.Models.Response;
using Google.Protobuf.WellKnownTypes;
using InventoryService;
using DM.DomainModels;
using BS.CustomExceptions.Common;

namespace BS.Services.InventoryManagementService
{
    public class InventoryManagementService : IInventoryManagementService
    {
        IUnitOfWork _unit;
        IConfiguration _configuration;

        public InventoryManagementService(IUnitOfWork unit, IConfiguration configuration) 
        {
            _unit = unit;
            _configuration = configuration;
        }


        public async Task<List<ResponseGetInventory>> GetInventoryData(string filter, CancellationToken cancellationToken)
        {
           List<ResponseGetInventory> response = new List<ResponseGetInventory>();
            var itemsGetter = await _unit.ItemsRepo.GetAsync(cancellationToken, x => x.IsActive == true,
                includeProperties: $"ItemImages");
            var items = itemsGetter?.Data?.Select(x=>new ResponseGetInventory
                {
                    ItemId = x.Id,
                    ItemName = x.ItemName,
                    ImagePath = x.ItemImages?.FirstOrDefault()?.ImagePath ?? "",
                    ItemCode = x.ItemCode,
                    Price = x.Price,
                    Quantity = x.Quantity
                });
            if(items == null)
            {
                throw new RecordNotFoundException("No items are there in the inventory, try reloading the inventory.");
            }
            response = items.ToList();
           /*string? port = _configuration.GetSection("Hub:Port").Value;
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
            var request = new DataRequest()
            {
                Data = filter
            };

            var verify = await client.SendDataAsync(request, cancellationToken: cancellationToken);
            var response = new ResponseGetInventory()
            {
                Message = verify.Message,
                IsSuccess = true
            };*/

            return response;
        }


        public async Task<ResponseReloadInventory> ReloadInventory(CancellationToken cancellationToken)
        {
            var response = new ResponseReloadInventory()
            {
                InventoryItems = new List<InventoryItems>()
            };

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
                var client = new InventoryServiceGRPC.InventoryServiceGRPCClient(channel);

                var verify = await client.ReloadInventoryDataAsync(new Empty(), cancellationToken: cancellationToken);

                foreach(var item in verify.Items)
                {
                    response.InventoryItems.Add(new InventoryItems()
                    {
                        Name = item.ItemName,
                        Code = item.ItemCode,
                        Barcode = item.Barcode,
                        Price = item.Price,
                        Categories = item.Categories.ToList(),
                        ItemGroup = new ItemGroup()
                        {
                            Name = item.Group.GroupName,
                            Code = item.Group.GroupCode,
                        }
                    });
                }

            }
            catch (RpcException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in local", ex);
            }


            return response;
        }

    }
}
