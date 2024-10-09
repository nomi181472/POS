using BS.CustomExceptions.Common;
using BS.Services.StoreManagementService.Model.Request;
using BS.Services.StoreManagementService.Model.Response;
using DA;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.StoreManagementService
{
    public class StoreManagementService : IStoreManagementService
    {
        IUnitOfWork _unit;

        public StoreManagementService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<bool> AddStore(RequestAddStore request, CancellationToken cancellationToken)
        {
            var StorageLocationEntry = new StorageLocation()
            {
                Id = Guid.NewGuid().ToString(),
                ERPCode = request.ERPCode,
                ERPName = request.ERPName,
                City = request.City,
                State = request.State,
                Country = request.Country,
                Address = request.Address,
                LocCode = request.LocCode,
                CompanyCode = request.CompanyCode,
                IsActive = true
                
            };
            _unit.StorageLocationRepo.AddAsync(StorageLocationEntry, "", cancellationToken);

            var StoreEntry = new Store()
            {
                Id = Guid.NewGuid().ToString(),
                StoreCode = request.StoreCode,
                StoreName = request.StoreName,
                AdminUser = request.AdminUser,
                StorageLocation = StorageLocationEntry,
                PriceListId = request.PriceListId,
                IsActive = true
            };

            await _unit.StoreRepo.AddAsync(StoreEntry, "SampleUser" ,cancellationToken);
            await _unit.CommitAsync(cancellationToken);

            return true;
        }

        public async Task<bool> UpdateStore(RequestUpdateStore request, CancellationToken cancellationToken)
        {
            var result = await _unit.StoreRepo.GetSingleAsync(cancellationToken, x => x.Id == request.Id, includeProperties: "StorageLocation");

            if(result.Data == null)
            {
                throw new RecordNotFoundException("Invalid Id");
            }

            var entry = result.Data;

            entry.StoreCode = request.Code;
            entry.StoreName = request.Name;
            entry.AdminUser = request.AdminUser;
            entry.StorageLocation.Address = request.Address;

            await _unit.StoreRepo.UpdateAsync(entry, "SampleUser", cancellationToken);
            await _unit.CommitAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteStore(RequestDeleteStore request, CancellationToken cancellationToken)
        {
            var result = await _unit.StoreRepo.GetSingleAsync(cancellationToken, x => x.Id == request.Id, includeProperties : "StorageLocation");
            
            if(result.Data == null)
            {
                throw new RecordNotFoundException("Invalid Id");
            }

            var entry = result.Data;
            entry.IsActive = false;
            entry.StorageLocation.IsActive = false;

            await _unit.StoreRepo.UpdateAsync(entry, "", cancellationToken);
            await _unit.CommitAsync(cancellationToken);

            return true;
        }

        public async Task<List<ResponseListAllStore>> ListAllStore(CancellationToken cancellationToken)
        {
            var response = new List<ResponseListAllStore>();
            var result = await _unit.StoreRepo.GetAsync(cancellationToken, x => x.IsActive == false ,includeProperties: "StorageLocation").ConfigureAwait(false);

            foreach(var store in result.Data)
            {
                var entry = new ResponseListAllStore()
                {
                    Name = store.StoreName,
                    Code = store.StoreCode,
                    Address = store.StorageLocation.Address,
                };
                response.Add(entry);
            }

            return response;
        }
    }
}
