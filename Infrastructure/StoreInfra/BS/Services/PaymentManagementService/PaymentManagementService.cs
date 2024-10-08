using BS.EnumsAndConstants.Constant;
using BS.Services.PaymentManagementService.Models.Request;
using BS.Services.PaymentManagementService.Models.Response;
using BS.Services.SaleProcessingService.Models.Request;
using DA;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.PaymentManagementService
{
    public class PaymentManagementService : IPaymentManagementService
    {
        private IUnitOfWork _unitOfWork;
        public PaymentManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseAddSurchargeDiscount> AddSurchargeDiscount(CreateOrderRequest request, Order order, string userId, CancellationToken token)
        {
            //var orderResult = await _unitOfWork.OrderRepo.GetByIdAsync(order.Id, token);
            //var order = orderResult.Data;

            double remainingAmount = request.TotalAmount;
            order.TotalAmount = request.TotalAmount;
            order.PaidAmount = 0;
            ResponseAddSurchargeDiscount response = new ResponseAddSurchargeDiscount();
            Dictionary<string, string?> paymentMethods = _unitOfWork.PaymentMethodsRepo.GetAsync(token, x => x.IsActive).Result.Data.ToDictionary(x => x.Id, x => x.Name);

            foreach(var splitPayment in request.OrderSplitPayments)
            {
                if(paymentMethods.TryGetValue(splitPayment.PaymentMethodId ?? "", out string? value))
                {
                    if (value == KConstantPaymentMethods.DiscountVoucher)
                    {
                        // #0 DiscountVoucher.Type will be handled in LoyaltyProgramService that will be developed when req clear
                        // #1 IF DiscountVoucherCode not in DB THEN throw RecordNotFoundException
                        // #2 IF DiscountVoucherCode.Expiry < DateTime.Now THEN throw InvalidOperationException("Voucher is Expired")
                        // #3 IF Voucher.RedeemAmount > Voucher.MaxPointLimit THEN throw InvalidOperationException("Can't redeem beyond max limit")
                        // #4 IF Voucher.DiscountType == "Fixed" THEN remainingAmount = remainingAmount - (Voucher.AmountPerPoint * Voucher.RedeemAmount)
                        // #5 ELIF Voucher.DiscountType == "Pc" THEN remainingAmount = remainingAmount * (100 - (Voucher.AmountPerPoint * Voucher.RedeemAmount))
                    }

                    if (value == KConstantPaymentMethods.LoyaltyPoints)
                    {
                        // #0 TODO: LoyaltyCard.Type will be handled with points accumulation in LoyaltyProgramService that will be developed when req clear
                        // #1 IF LoyaltyPoints.RedeemAmount > LoyaltyPoints.MaxPointLimit THEN throw InvalidOperationException("Can't redeem beyond max limit")
                        // #2 ELSE remainingAmount = remainingAmount - ((LoyaltyPoints.AmountPerPoint * LoyaltyPoints.RedeemAmount) * CurrencyConversionRate)
                    }

                    if (value == KConstantPaymentMethods.Cheque)
                    {
                        double chequeSurchargeAmount = remainingAmount * 0.07;
                        response.ChequeSurchargeAmount = chequeSurchargeAmount;
                        remainingAmount += chequeSurchargeAmount;
                        remainingAmount -= splitPayment.PaidAmount;
                        order.PaidAmount += splitPayment.PaidAmount;
                    }

                    if (value == KConstantPaymentMethods.OnlineTransfer)
                    {
                        double onlineTransferSurchargeAmount = remainingAmount * 0.07;
                        response.OnlineTransferSurchargeAmount = onlineTransferSurchargeAmount;
                        remainingAmount += onlineTransferSurchargeAmount;
                        remainingAmount -= splitPayment.PaidAmount;
                        order.PaidAmount += splitPayment.PaidAmount;
                    }

                    if (value == KConstantPaymentMethods.Cash)
                    {
                        double discountAmount = splitPayment.PaidAmount * 0.07;
                        response.CashDiscountAmount = discountAmount;
                        remainingAmount -= discountAmount;
                        remainingAmount -= splitPayment.PaidAmount;
                        order.PaidAmount += splitPayment.PaidAmount;
                    }

                    if (value == KConstantPaymentMethods.Card)
                    {
                        double cardSurchargeAmount = remainingAmount * 0.07;
                        response.CardSurchargeAmount = cardSurchargeAmount;
                        remainingAmount += cardSurchargeAmount;
                        // TODO: Pass remaining amount to bool PayWCard()
                        remainingAmount -= splitPayment.PaidAmount;

                        order.PaidAmount += splitPayment.PaidAmount;
                    }

                    /*if (remainingAmount > 0)
                    {
                        if (request.IsCredit == true)
                        {
                            response.CreditAmount = remainingAmount;
                        }
                        else
                        {
                            response.RemainingAmounnt = remainingAmount;
                        }
                    }*/
                }
            }

            await _unitOfWork.CommitAsync();
            return response;
        }
    }
}
