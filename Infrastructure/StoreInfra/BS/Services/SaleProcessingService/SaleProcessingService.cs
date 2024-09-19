using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.PaymentManagementService;
using BS.Services.SaleProcessingService.Models.Request;
using BS.Services.SaleProcessingService.Models.Response;
using DA;
using DM.DomainModels;

namespace BS.Services.SaleProcessingService
{
    public class SaleProcessingService:ISaleProcessingService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IPaymentManagementService _paymentManagementService;
        public SaleProcessingService(IUnitOfWork unitOfWork, IPaymentManagementService paymentManagementService)
        {
            _unitOfWork = unitOfWork;
            _paymentManagementService = paymentManagementService;
        }
        public async Task<CreateCartResponse> CreateCart(CreateCartRequest request, string userId, CancellationToken cancellationToken)
        {
            CreateCartResponse createCartResponse = new CreateCartResponse();
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The request can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                throw new ArgumentNullException(nameof(request), "Customer Id can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.TillId))
            {
                throw new ArgumentNullException(nameof(request), "Till Id can not be null.");
            }
            string cartId = Guid.NewGuid().ToString();
            var response = await _unitOfWork.CustomerCartRepo.AddAsync(new DM.DomainModels.CustomerCart
            {
                Id = cartId,
                CustomerId = request.CustomerId,
                IsConvertedToSale = request.IsConvertedToSale,
                IsActive = true,
                IsArchived = false,
                TillId = request.TillId
            },userId, cancellationToken);
            if (response.IsException)
            {
                throw new Exception(ExceptionMessage.SWW);
            }
            await _unitOfWork.CommitAsync(cancellationToken);
            createCartResponse.CartId = cartId;
            return createCartResponse;
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
        public async Task<List<Carts>> GetActiveCartsByTill(string tillId, CancellationToken cancellationToken)
        {
            List<Carts> carts = new List<Carts>();
            if (string.IsNullOrWhiteSpace(tillId))
            {
                throw new ArgumentNullException("The Till Id can not be null.");
            }
            carts = _unitOfWork.CustomerCartRepo
                .GetAsync(cancellationToken, x => x.TillId == tillId && x.IsActive == true && x.IsConvertedToSale == false, 
                orderBy: q => q.OrderByDescending(p => p.CreatedDate)).Result.Data
                .Select(x=>new Carts { Id = x.Id, CustomerId = x.CustomerId }).ToList();
            if(carts == null)
            {
                throw new RecordNotFoundException("No active carts found for the till.");
            }
            if(carts.Count == 0)
            {
                throw new RecordNotFoundException("No active carts found for the till.");
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
            if(request.Items == null)
            {
                throw new ArgumentNullException("Item Id is required.");
            }
            if (request.Items.Count == 0)
            {
                throw new ArgumentNullException("Item Ids are required.");
            }
            var cart = await _unitOfWork.CustomerCartRepo.GetByIdAsync(request.CartId, cancellationToken);
            if(cart.Data == null)
            {
                throw new RecordNotFoundException("The cart does not exist.");
            }

            foreach (var item in request.Items)
            {
                if(item.Quantity > 0)
                {
                    await _unitOfWork.CustomerCartItemsRepo.AddAsync(new CustomerCartItems
                    {
                        Id = Guid.NewGuid().ToString(),
                        CartId = request.CartId,
                        IsActive = true,
                        IsArchived = false,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity ?? 0
                    }, userId, cancellationToken);
                }
            }
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }

        public async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request, string userId, CancellationToken cancellationToken)
        {
            CreateOrderResponse createOrderResponse = new CreateOrderResponse();

            #region Request validation
            if (request == null)
            {
                throw new ArgumentNullException("The request can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.CartId))
            {
                throw new ArgumentNullException("Cart Id can not be null.");
            }
            if(request.OrderSplitPayments == null)
            {
                throw new ArgumentNullException("Payment method is required.");
            }
            if(request.OrderSplitPayments.Count == 0)
            {
                throw new ArgumentNullException("Payment method is required.");
            }
            if (request.TotalAmount <= 0)
            {
                throw new ArgumentNullException("Total amount must be greater than 0.");
            }
            #endregion

            #region Create Order
            string orderId = Guid.NewGuid().ToString();
            var orderInit = new Order
            {
                Id = orderId,
                CartId = request.CartId,
                PaidAmount = 0,
                TotalAmount = request.TotalAmount,
                IsArchived = false,
                IsActive = true,
                IsPaid = false
            };
            await _unitOfWork.OrderRepo.AddAsync(orderInit, userId, cancellationToken);
            foreach(var splitPayment in request.OrderSplitPayments)
            {
                await _unitOfWork.OrderSplitPaymentsRepo.AddAsync(new OrderSplitPayments
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = orderId,
                    PaymentMethodId = splitPayment.PaymentMethodId,
                    Amount = splitPayment.PaidAmount,
                    IsArchived = false,
                    IsActive = true
                }, userId, cancellationToken);
            }
            await _unitOfWork.CommitAsync(cancellationToken);
            #endregion
            await _paymentManagementService.AddSurchargeDiscount(request, orderInit, userId, cancellationToken);
            var order = await _unitOfWork.OrderRepo.GetByIdAsync(orderId, cancellationToken);
            if (order.Data == null)
            {
                throw new RecordNotFoundException("Something went wrong");
            }
            #region Return response
            if (order.Data.PaidAmount == order.Data.TotalAmount)
            {
                order.Data.IsPaid = true;
                await _unitOfWork.OrderRepo.UpdateAsync(order.Data, userId, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);
                createOrderResponse.TotalAmount = order.Data.TotalAmount;
                createOrderResponse.PaidAmount = order.Data.PaidAmount;
                createOrderResponse.IsPaid = order.Data.IsPaid;
                createOrderResponse.Success = true;
                createOrderResponse.Message = "Sale processed successfully.";
                return createOrderResponse;
            }
            else
            {
                createOrderResponse.TotalAmount = order.Data.TotalAmount;
                createOrderResponse.PaidAmount = order.Data.PaidAmount;
                createOrderResponse.IsPaid = order.Data.IsPaid;
                createOrderResponse.Success = false;
                createOrderResponse.Message = $"Sale was not closed, only {order.Data.PaidAmount} of {order.Data.TotalAmount} was processed.";
                return createOrderResponse;
            }
            #endregion
        }
    }
}
