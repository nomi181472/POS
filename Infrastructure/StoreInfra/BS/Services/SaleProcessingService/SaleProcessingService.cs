using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.SaleProcessingService.Models.Request;
using BS.Services.SaleProcessingService.Models.Response;
using DA;
using DM.DomainModels;

namespace BS.Services.SaleProcessingService
{
    public class SaleProcessingService:ISaleProcessingService
    {
        private IUnitOfWork _unitOfWork;
        public SaleProcessingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateCart(CreateCartRequest request, string userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The request can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                throw new ArgumentNullException(nameof(request), "Customer Id can not be null.");
            }
            var response = await _unitOfWork.CustomerCartRepo.AddAsync(new DM.DomainModels.CustomerCart
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = request.CustomerId,
                IsConvertedToSale = request.IsConvertedToSale,
                IsActive = true,
                IsArchived = false
            },userId, cancellationToken);
            if (response.IsException)
            {
                throw new Exception(ExceptionMessage.SWW);
            }
            await _unitOfWork.CommitAsync(cancellationToken);
            return response.Result;
        }
        public async Task<bool> UpdateCart(UpdateCartRequest request, string userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The request can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                throw new ArgumentNullException(nameof(request), "Customer Id can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.CartId))
            {
                throw new ArgumentNullException(nameof(request), "Cart Id can not be null.");
            }

            CustomerCart cart = _unitOfWork.CustomerCartRepo.GetByIdAsync(request.CartId, cancellationToken).Result.Data;
            if (cart == null)
            {
                throw new RecordNotFoundException("Invalid Cart Id.");
            }
            cart.IsConvertedToSale = request.IsConvertedToSale;
            cart.CustomerId = request.CustomerId;
            var response = await _unitOfWork.CustomerCartRepo.UpdateAsync(cart, userId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return response.Result;
        }
        public async Task<bool> RemoveCart(RemoveCartRequest request, string userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The request can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.CartId))
            {
                throw new ArgumentNullException(nameof(request), "Cart Id can not be null.");
            }
            CustomerCart cart = _unitOfWork.CustomerCartRepo.GetByIdAsync(request.CartId, cancellationToken).Result.Data;
            if (cart == null)
            {
                throw new RecordNotFoundException("The cart you are trying to remove does not exist.");
            }
            cart.IsActive = false;
            var response = await _unitOfWork.CustomerCartRepo.UpdateAsync(cart, userId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return response.Result;
        }
        public async Task<List<Carts>> GetActiveCartsByUser(string userId, CancellationToken cancellationToken)
        {
            List<Carts> carts = new List<Carts>();
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("The User Id can not be null.");
            }
            carts = _unitOfWork.CustomerCartRepo
                .GetAsync(cancellationToken, x => x.CreatedBy == userId && x.IsActive == true && x.IsConvertedToSale == false, 
                orderBy: q => q.OrderByDescending(p => p.CreatedDate)).Result.Data
                .Select(x=>new Carts { Id = x.Id, CustomerId = x.CustomerId }).ToList();
            if(carts == null)
            {
                throw new RecordNotFoundException("No active carts found for the user.");
            }
            if(carts.Count == 0)
            {
                throw new RecordNotFoundException("No active carts found for the user.");
            }
            return carts;
        }
        public async Task<bool> AddItemsToCart(AddItemsToCartRequest request, string userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The request can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.CartId))
            {
                throw new ArgumentNullException("Cart Id can not be null.");
            }
            if(request.ItemIds == null)
            {
                throw new ArgumentNullException("Item Id is required.");
            }
            if (request.ItemIds.Count == 0)
            {
                throw new ArgumentNullException("Item Ids are required.");
            }
            var cart = await _unitOfWork.CustomerCartRepo.GetByIdAsync(request.CartId, cancellationToken);
            if(cart.Data == null)
            {
                throw new RecordNotFoundException("The cart does not exist.");
            }

            foreach (var item in request.ItemIds)
            {
                await _unitOfWork.CustomerCartItemsRepo.AddAsync(new CustomerCartItems { Id = Guid.NewGuid().ToString(),CartId = request.CartId, 
                    IsActive = true, IsArchived = false, ItemId = item }, userId, cancellationToken);
            }
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
