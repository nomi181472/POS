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
using Microsoft.EntityFrameworkCore;

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
                throw new ArgumentException("The request can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                throw new ArgumentException("Customer Id can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.TillId))
            {
                throw new ArgumentException("Till Id can not be null.");
            }
            var customer = _unitOfWork.CustomerManagementRepo.GetAsync(cancellationToken, x => x.Id == request.CustomerId.Trim() && x.IsActive == true).Result.Data.FirstOrDefault();
            if(customer == null)
            {
                throw new KeyNotFoundException("Invalid Customer Id.");
            }
            var till = _unitOfWork.TillRepo.GetAsync(cancellationToken, x => x.Id == request.TillId.Trim() && x.IsActive == true).Result.Data.FirstOrDefault();
            if (till == null)
            {
                throw new KeyNotFoundException("Invalid Till Id.");
            }
            string cartId = Guid.NewGuid().ToString();
            var prevCart = _unitOfWork.CustomerCartRepo.GetAsync(cancellationToken, x=>x.CustomerId == request.CustomerId.Trim() && x.IsActive==true && x.IsConvertedToSale == false).Result.Data.FirstOrDefault();
            if(prevCart != null)
            {
                throw new RecordAlreadyExistException("Active cart for this customer already exists.");
            }
            var response = await _unitOfWork.CustomerCartRepo.AddAsync(new DM.DomainModels.CustomerCart
            {
                Id = cartId,
                CustomerId = request.CustomerId.Trim(),
                IsConvertedToSale = request.IsConvertedToSale,
                IsActive = true,
                IsArchived = false,
                TillId = request.TillId.Trim()
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
                throw new ArgumentException("The request can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                throw new ArgumentException("Customer Id can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.CartId))
            {
                throw new ArgumentException("Cart Id can not be null.");
            }
            var customer = _unitOfWork.CustomerManagementRepo.GetAsync(cancellationToken, x => x.Id == request.CustomerId.Trim() && x.IsActive == true).Result.Data.FirstOrDefault();
            if (customer == null)
            {
                throw new KeyNotFoundException("Invalid Customer Id.");
            }
            CustomerCart? cart = _unitOfWork.CustomerCartRepo.GetAsync(cancellationToken, 
                x => x.Id == request.CartId.Trim() && x.IsActive == true).Result.Data.FirstOrDefault();
            if (cart == null)
            {
                throw new RecordNotFoundException("Invalid Cart Id.");
            }
            if(cart.IsConvertedToSale == true)
            {
                throw new RecordNotFoundException("The cart you are trying to update has already been converted to sale.");
            }

            var customerPrevCart = _unitOfWork.CustomerCartRepo.GetAsync(cancellationToken, 
                x => x.CustomerId == request.CustomerId.Trim() && x.Id != request.CartId.Trim()
                 && x.IsActive == true && x.IsConvertedToSale == false).Result.Data.FirstOrDefault();
            if(customerPrevCart != null)
            {
                throw new ArgumentException("The customer you are trying to assign the cart already has an active cart.");
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
                throw new ArgumentException("The request can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.CartId))
            {
                throw new ArgumentException("Cart Id can not be null.");
            }
            CustomerCart? cart = _unitOfWork.CustomerCartRepo.GetAsync(cancellationToken,x => x.Id == request.CartId.Trim() && x.IsActive == true).Result.Data.FirstOrDefault();
            if (cart == null)
            {
                throw new RecordNotFoundException("The cart you are trying to remove does not exist.");
            }
            if (cart.IsConvertedToSale)
            {
                throw new ArgumentException("Unable to delete the cart that has been converted to sale.");
            }
            cart.IsActive = false;
            var response = await _unitOfWork.CustomerCartRepo.UpdateAsync(cart, userId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return response.Result;
        }
        public async Task<List<Carts>> GetActiveCartsByTill(string tillId, CancellationToken cancellationToken)
        {
            List<Carts>? carts = new List<Carts>();
            tillId = tillId.Trim();
            if (string.IsNullOrWhiteSpace(tillId))
            {
                throw new ArgumentException("The Till Id can not be null.");
            }
            var till = _unitOfWork.TillRepo.GetAsync(cancellationToken, x => x.Id == tillId && x.IsActive == true).Result.Data.FirstOrDefault();
            if (till == null)
            {
                throw new KeyNotFoundException("Invalid Till Id.");
            }
            int totalAmount = 0;
            carts = _unitOfWork.CustomerCartRepo
                .GetAsync(cancellationToken, x => x.TillId == tillId && x.IsActive == true && x.IsConvertedToSale == false, 
                orderBy: q => q.OrderByDescending(p => p.CreatedDate), includeProperties: $"{nameof(CustomerManagement)},{nameof(CustomerCartItems)}.{nameof(Items)}")?.Result?.Data?
                .Select(x=>new Carts { 
                    Id = x.Id, 
                    CustomerId = x.CustomerId,
                    CustomerName = x?.CustomerManagement?.Name ?? "",
                    TotalItems = x?.CustomerCartItems?.Count(y=>y.CartId == x.Id && y.IsActive==true),
                    TotalAmount = x?.CustomerCartItems?.Where(y => y.CartId == x.Id && y.IsActive == true).Sum(x=>x?.Items?.Price * x?.Quantity ?? 0)
                }).ToList();
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
                throw new ArgumentException("The request can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.CartId))
            {
                throw new ArgumentException("Cart Id can not be null.");
            }
            if(request.Items == null)
            {
                throw new ArgumentException("Item Id is required.");
            }
            if (request.Items.Count == 0)
            {
                throw new ArgumentException("Item Id is required.");
            }
            if (!request.Items.All(x => _unitOfWork.ItemsRepo.GetQueryable().Data.Select(x=>x.Id).ToList().Contains(x.ItemId?.Trim() ?? "")))
            {
                throw new RecordNotFoundException("Some of the items you are trying to add does not exist.");
            }

            var cart = await _unitOfWork.CustomerCartRepo.GetAsync(cancellationToken, x => x.Id == request.CartId.Trim() && 
            x.IsActive == true && x.IsConvertedToSale==false,
            includeProperties:$"{nameof(CustomerCartItems)}");

            if(cart.Data == null)
            {
                throw new RecordNotFoundException("The cart does not exist.");
            }
            if(cart.Data.Count() == 0)
            {
                throw new RecordNotFoundException("The cart does not exist.");
            }
            var cartItems = cart.Data.FirstOrDefault()?.CustomerCartItems?.Where(x => x.IsActive == true).ToList();
            if(cartItems != null)
            {
                foreach (var item in cartItems)
                {
                    item.IsActive = false;
                    await _unitOfWork.CustomerCartItemsRepo.UpdateAsync(item, userId, cancellationToken);
                }
            }
            foreach (var item in request.Items)
            {
                if (string.IsNullOrWhiteSpace(item.ItemId))
                {
                    throw new ArgumentException("Item Id is required.");
                }
                if (item.Quantity == null)
                {
                    throw new ArgumentException("Item quantity is required.");
                }
                if (item.Quantity > 0)
                {
                    await _unitOfWork.CustomerCartItemsRepo.AddAsync(new CustomerCartItems
                    {
                        Id = Guid.NewGuid().ToString(),
                        CartId = request.CartId.Trim(),
                        IsActive = true,
                        IsArchived = false,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity ?? 0
                    }, userId, cancellationToken);
                }
                else
                {
                    throw new ArgumentException("Item quantity must be greater than 0 for all the items.");
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
                throw new ArgumentException("The request can not be null.");
            }
            if (string.IsNullOrWhiteSpace(request.CartId))
            {
                throw new ArgumentException("Cart Id can not be null.");
            }
            if(request.OrderSplitPayments == null)
            {
                throw new ArgumentException("Payment method is required.");
            }
            if(request.OrderSplitPayments.Count == 0)
            {
                throw new ArgumentException("Payment method is required.");
            }
            if (request.TotalAmount <= 0)
            {
                throw new ArgumentException("Total amount must be greater than 0.");
            }
            #endregion

            #region Create Order
            string orderId = Guid.NewGuid().ToString();
            var orderInit = new Order
            {
                Id = orderId,
                CartId = request.CartId.Trim(),
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
                    PaymentMethodId = splitPayment.PaymentMethodId?.Trim(),
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
        public async Task<ViewCartResponse> ViewCart(string? cartId, string userId, CancellationToken cancellationToken)
        {
            ViewCartResponse response = new ViewCartResponse();
            cartId = cartId?.Trim() ?? "";
            var cartResult = await _unitOfWork.CustomerCartRepo.GetSingleAsync(cancellationToken, 
                x => x.Id == cartId && x.IsActive == true,
                includeProperties: $"{nameof(CustomerCartItems)}.{nameof(Items)}");
            CustomerCart cart = cartResult.Data;
            if(cart == null)
            {
                throw new ArgumentException("Cart was not found.");
            }
            if (cart.IsConvertedToSale)
            {
                throw new ArgumentException("Cart has already been converted to sale.");
            }
            response.CartItems = cart.CustomerCartItems?.Where(x => x.IsActive).Select(x => new CartItems
            {
                ItemId = x.ItemId,
                ItemCode = x.Items?.ItemCode ?? "",
                ItemName = x.Items?.ItemName ?? "",
                Price = x.Items?.Price ?? 0,
                Quantity = x.Quantity,
            }).ToList();
            response.CustomerId = cart.CustomerId;
            return response;
        }
    }
}
