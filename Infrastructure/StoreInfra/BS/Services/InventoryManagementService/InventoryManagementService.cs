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
using ItemGroup = DM.DomainModels.ItemGroup;

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
                    Quantity = x.Quantity,
                    //ItemGroupDetails = new ItemGroupDetails()
                    //{
                    //    GroupId = x.ItemGroup?.Id ?? "",
                    //    GroupName = x.ItemGroup?.Name ?? "",
                    //    GroupCode = x.ItemGroup?.GroupCode ?? ""
                    //},
                    //ItemTaxDetails = new ItemTaxDetails()
                    //{
                    //    TaxId = x.Taxes?.Id ?? "",
                    //    TaxCode = x.Taxes?.TaxCode ?? "",
                    //    Percentage = x.Taxes?.Percentage ?? 0
                    //}
                    
                });

            if(items == null)
            {
                throw new RecordNotFoundException("No items are there in the inventory, try reloading the inventory.");
            }

            response = items.ToList();
            return response;
        }


        public async Task<ResponseReloadInventory> ReloadInventory(CancellationToken cancellationToken)
        {
            var response = new ResponseReloadInventory()
            {
                InventoryItemsAdded = new List<InventoryItemsAdded>()
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
                    var ItemGroupEntry = new ItemGroup()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = item.Group.GroupName,
                        GroupCode = item.Group.GroupCode,
                    };
                    await _unit.ItemGroupRepo.AddAsync(ItemGroupEntry, "", cancellationToken);

                    var ItemEntry = new Items()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        ItemGroup = ItemGroupEntry
                    };
                    await _unit.ItemsRepo.AddAsync(ItemEntry, "", cancellationToken);

                    var ItemImageEntry = new ItemImage()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ItemCode = item.ItemCode,
                        ImagePath = item.ImagePath,
                        Items = ItemEntry
                    };
                    await _unit.ItemImageRepo.AddAsync(ItemImageEntry, "", cancellationToken);

                    var ItemTaxEntry = new Tax()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Items = ItemEntry
                    };
                    await _unit.TaxRepo.AddAsync(ItemTaxEntry, "", cancellationToken);

                    await _unit.CommitAsync(cancellationToken);

                    response.InventoryItemsAdded.Add(new InventoryItemsAdded()
                    {
                        Name = item.ItemName,
                        Code = item.ItemCode,
                        Barcode = item.Barcode,
                        Price = item.Price,
                        Categories = item.Categories.ToList(),
                        Item_Group = new Item_Group()
                        {
                            Name = item.Group.GroupName,
                            Code = item.Group.GroupCode,
                        },
                        Tax = new Tax_Detail()
                        {
                           TaxCode = item.Tax.TaxCode,
                           Percentage = item.Tax.Percentage,
                        },
                        ImagePath = item.ImagePath,
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
