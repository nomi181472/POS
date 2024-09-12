using BS.CustomExceptions.Common;
using BS.Services.AreaCoverageManagementService.Model.Request;
using BS.Services.AreaCoverageManagementService.Model.Response;
using BS.Services.InventoryManagementService.Model.Request;
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
                ItemResponse = new List<ItemResponseDetails>()  // Ensure the list is initialized
            };

            foreach (var item in request.Items)
            {
                try
                {
                    var entity = new InventoryItems()
                    {
                        Name = item.ItemName,
                        Code = item.ItemCode,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = userId,
                        UpdatedDate = DateTime.Now,
                        IsActive = true,
                        IsArchived = false,

                    };

                    var result1 = _unit.InventoryItemsRepo.GetSingle(x => x.Code.ToLower() == item.ItemCode.ToLower());

                    if (IsItemExist(item.ItemCode))
                    {
                        _unit.InventoryItemsRepo.UpdateAsync(entity, userId, token);
                    }
                    else
                    {
                        entity.Id = Guid.NewGuid().ToString();
                        _unit.InventoryItemsRepo.AddAsync(entity, userId, token);
                    }


                    response.ItemResponse.Add(new ItemResponseDetails
                    {
                        Id = entity.Id,
                        Code = entity.Code,
                        Name = entity.Name,
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
                        Status = $"Failed: {ex.Message}"
                    });
                }
            }

            _unit.CommitAsync(token);

            return response;
        }


        public bool IsItemExist(string Code)
        {
            var result = _unit.InventoryItemsRepo.Any(x => x.Code.ToLower() == Code.ToLower() && x.IsActive);
            if (result.Status)
            {
                return result.Data;
            }
            else
            {
                throw new UnknownException(result.Message);
            }
        }
    }
}
