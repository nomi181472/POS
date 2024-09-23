﻿using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InventoryService;

namespace Hub.HubGrpc.Server
{
    public class InventoryGrpcImpl : InventoryServiceGRPC.InventoryServiceGRPCBase
    {
        public override async Task<ReloadInventoryResponse> ReloadInventoryData(Empty request, ServerCallContext context)
        {
            var response = new ReloadInventoryResponse();

            response.Items.Add(new InventoryItem
            {
                ItemCode = "001",
                ItemName = "Product A",
                Barcode = "123456789",
                Price = 10.99,
                Group = new InventoryGroup
                {
                    GroupCode = "G001",
                    GroupName = "Electronics"
                },
                Categories = { "Category1", "Category2" }
            });

            response.Items.Add(new InventoryItem
            {
                ItemCode = "002",
                ItemName = "Product B",
                Barcode = "987654321",
                Price = 15.49,
                Group = new InventoryGroup
                {
                    GroupCode = "G002",
                    GroupName = "Furniture"
                },
                Categories = { "Category3", "Category4" }
            });

            return response;
        }
    }
}