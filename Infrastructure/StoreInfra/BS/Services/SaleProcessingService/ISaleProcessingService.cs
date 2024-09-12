using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.Services.SaleProcessingService.Models.Request;
using BS.Services.SaleProcessingService.Models.Response;

namespace BS.Services.SaleProcessingService
{
    public interface ISaleProcessingService
    {
        public Task<bool> CreateCart(CreateCartRequest request, string userId, CancellationToken cancellationToken);
        public Task<bool> UpdateCart(UpdateCartRequest request, string userId, CancellationToken cancellationToken);
        public Task<bool> RemoveCart(RemoveCartRequest request, string userId, CancellationToken cancellationToken);
        public Task<List<Carts>> GetActiveCartsByUser(string userId, CancellationToken cancellationToken);
        public Task<bool> AddItemsToCart(AddItemsToCartRequest request, string userId, CancellationToken cancellationToken);
    }
}