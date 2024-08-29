using BS.Services.AreaCoverageManagementService.Model.Request;
using BS.Services.AreaCoverageManagementService.Model.Response;
using DA;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BS.Services.AreaCoverageManagementService
{
    public class AreaCoverageManagementService : IAreaCoverageManagementService
    {
        IUnitOfWork _unit;
        public AreaCoverageManagementService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<ResponseAddArea> AddArea(RequestAddArea request, string userId, CancellationToken cancellationToken)
        {
            var entity = new Location()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Address = request.Address,
                City = request.City,
                Province = request.Province,
                UpdatedBy = userId,
                UpdatedDate = DateTime.Now,
                CreatedBy = userId,
                CreatedDate = DateTime.Now,
            };

            await _unit.LocationRepo.AddAsync(entity, userId, cancellationToken);
            await _unit.CommitAsync(cancellationToken);


            return new ResponseAddArea() 
            { 
                Address = entity.Address,
                Id = entity.Id,
            };
        }
    }
}
