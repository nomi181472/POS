using BS.CustomExceptions.Common;
using BS.Services.AreaCoverageManagementService.Model.Request;
using BS.Services.AreaCoverageManagementService.Model.Response;
using BS.Services.InventoryManagementService.Model.Request;
using BS.Services.InventoryManagementService.Model.Response;
using DA;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.InventoryManagementService
{
    public class InventoryManagementService: IInventoryManagementService
    {
        IUnitOfWork _unit;
        public InventoryManagementService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<ResponseAddItemData> AddItemData(RequestAddItemData request, string userId, CancellationToken token)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = new ResponseAddItemData
            {
                ItemResponse = new List<ItemResponseDetails>()
            };

            foreach (var item in request.Items)
            {
                try
                {
                    if (IsItemExist(item.ItemCode))
                    {
                        throw new Exception("Item Already exists");
                    }

                    var entity = new Items()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ItemName = item.ItemName,
                        ItemCode = item.ItemCode,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = userId,
                        UpdatedDate = DateTime.Now,
                        IsActive = true,
                        IsArchived = false,

                    };

                    await _unit.InventoryItemsRepo.AddAsync(entity, userId, token);
                    await _unit.CommitAsync(token);

                    response.ItemResponse.Add(new ItemResponseDetails
                     {
                         Id = entity.Id,
                         Code = entity.ItemCode,
                         Name = entity.ItemName,
                         IsSuccess = true,
                         Status = "Success"
                     });
                }
                catch (Exception ex)
                {
                    response.ItemResponse.Add(new ItemResponseDetails
                    {
                        Id = null,  // No ID because the item failed to be added
                        Code = item.ItemCode,
                        Name = item.ItemName,
                        IsSuccess = false,
                        Status = $"Failed: {ex.Message}"
                    });
                }
            }

            return response;
        }


        public bool IsItemExist(string Code)
        {
            var result = _unit.InventoryItemsRepo.Any(x => x.ItemCode == Code && x.IsActive);
            if (result.Status)
            {
                return result.Data;
            }
            else
            {
                throw new UnknownException(result.Message);
            }
        }

/*        public bool IsItemGroupExist(int Code)
        {
            var result = _unit.InventoryGroupsRepo.Any(x => x.Code == Code && x.IsActive);
            if (result.Status)
            {
                return result.Data;
            }
            else
            {
                throw new UnknownException(result.Message);
            }
        }*/


        public async Task<ResponseUpdateItemData> UpdateItemData(RequestUpdateItemData request, string userId, CancellationToken token)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = new ResponseUpdateItemData
            {
                ItemResponse = new List<ItemResponseDetails>()
            };

            foreach (var item in request.Items)
            {
                try
                {
                    var result = await _unit.InventoryItemsRepo.GetSingleAsync(token, x => x.ItemCode == item.ItemCode && x.IsActive);


                    result.Data.ItemName = item.ItemName;
                    result.Data.ItemCode = item.ItemCode;
                    result.Data.UpdatedBy = userId;
                    result.Data.UpdatedDate = DateTime.Now;

                    await _unit.InventoryItemsRepo.UpdateAsync(result.Data, userId, token);
                    await _unit.CommitAsync(token);


                    response.ItemResponse.Add(new ItemResponseDetails
                    {
                        Id = result.Data.Id,
                        Code = result.Data.ItemCode,
                        Name = result.Data.ItemName,
                        IsSuccess = true,
                        Status = "Success"
                    });


                }
                catch (Exception ex)
                {
                    response.ItemResponse.Add(new ItemResponseDetails
                    {
                        Id = null,  // No ID because the item failed to be added
                        Code = item.ItemCode,
                        Name = item.ItemName,
                        IsSuccess = false,
                        Status = $"Failed: {ex.Message}"
                    });
                }
            }

            return response;

        }

        /*public async Task<ResponseAddItemGroup> AddItemGroup(RequestAddItemGroup request, string userId, CancellationToken token)
        {
            var response = new ResponseAddItemGroup()
            {
                ItemGrpsResponse = new List<ItemGrpResponseDetails>()
            };

            if (request == null) throw new ArgumentNullException(
                nameof(request));


            foreach (var itemgrp in request.ItemGroups)
                {
                try
                {
                    if (IsItemGroupExist(itemgrp.Code))
                    {
                        throw new Exception("Group Already Exists");
                    }

                    var entity = new InventoryGroups()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = itemgrp.Name,
                        Code = itemgrp.Code,
                        CreatedBy = userId,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedBy = userId,
                        UpdatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsArchived = false,
                    };

                    _unit.InventoryGroupsRepo.AddAsync(entity, userId, token);
                    _unit.CommitAsync(token);

                    response.ItemGrpsResponse.Add(new ItemGrpResponseDetails
                    {
                        Id = entity.Id,  // No ID because the item failed to be added
                        Code = entity.Code,
                        Name = entity.Name,
                        IsSuccess = true,
                        Status = "Success"
                    });

                }
                catch (Exception ex)
                {
                    response.ItemGrpsResponse.Add(new ItemGrpResponseDetails
                    {
                        Id = null,  // No ID because the item failed to be added
                        Code = itemgrp.Code,
                        Name = itemgrp.Name,
                        IsSuccess = false,
                        Status = $"Failed: {ex.Message}"
                    });
                }
            }
            


            return response;
        }*/

    }
}
