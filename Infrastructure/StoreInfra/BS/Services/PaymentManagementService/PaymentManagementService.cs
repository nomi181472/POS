using BS.Services.PaymentManagementService.Models.Request;
using BS.Services.PaymentManagementService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.PaymentManagementService
{
    public class PaymentManagementService : IPaymentManagementService
    {
        public async Task<ResponseAddSurchargeDiscount> AddSurchargeDiscount(RequestAddSurchargeDiscount request, string userId, CancellationToken token)
        {
            double remainingAmount = request.TotalBill;
            ResponseAddSurchargeDiscount response = new ResponseAddSurchargeDiscount();

            if (request.DiscountVoucherCode != null)
            {
                // #0 DiscountVoucher.Type will be handled in LoyaltyProgramService that will be developed when req clear
                // #1 IF DiscountVoucherCode not in DB THEN throw RecordNotFoundException
                // #2 IF DiscountVoucherCode.Expiry < DateTime.Now THEN throw InvalidOperationException("Voucher is Expired")
                // #3 IF Voucher.RedeemAmount > Voucher.MaxPointLimit THEN throw InvalidOperationException("Can't redeem beyond max limit")
                // #4 IF Voucher.DiscountType == "Fixed" THEN remainingAmount = remainingAmount - (Voucher.AmountPerPoint * Voucher.RedeemAmount)
                // #5 ELIF Voucher.DiscountType == "Pc" THEN remainingAmount = remainingAmount * (100 - (Voucher.AmountPerPoint * Voucher.RedeemAmount))
            }

            if (request.LoyaltyPointsRedeemed != 0)
            {
                // #0 TODO: LoyaltyCard.Type will be handled with points accumulation in LoyaltyProgramService that will be developed when req clear
                // #1 IF LoyaltyPoints.RedeemAmount > LoyaltyPoints.MaxPointLimit THEN throw InvalidOperationException("Can't redeem beyond max limit")
                // #2 ELSE remainingAmount = remainingAmount - ((LoyaltyPoints.AmountPerPoint * LoyaltyPoints.RedeemAmount) * CurrencyConversionRate)
            }

            if(request.ChequeAmount != 0)
            {
                double chequeSurchargeAmount = remainingAmount * 0.07;
                response.ChequeSurchargeAmount = chequeSurchargeAmount;
                remainingAmount += chequeSurchargeAmount;
                remainingAmount -= request.ChequeAmount;
            }

            if (request.OnlineTransferAmount != 0)
            {
                double onlineTransferSurchargeAmount = remainingAmount * 0.07;
                response.OnlineTransferSurchargeAmount = onlineTransferSurchargeAmount;
                remainingAmount += onlineTransferSurchargeAmount;
                remainingAmount -= request.OnlineTransferAmount;
            }

            if (request.CashAmount != 0)
            {
                double discountAmount = request.CashAmount * 0.07;
                response.CashDiscountAmount = discountAmount;
                remainingAmount -= discountAmount;
                remainingAmount -= request.CashAmount;
            }

            if(request.CardAmount != 0)
            {
                double cardSurchargeAmount = remainingAmount * 0.07;
                response.CardSurchargeAmount = cardSurchargeAmount;
                remainingAmount += cardSurchargeAmount;
                // TODO: Pass remaining amount to bool PayWCard()
                remainingAmount -= request.CardAmount;
            }

            if (remainingAmount > 0)
            {
                if (request.IsCredit == true)
                {
                    response.CreditAmount = remainingAmount;
                }
                else
                {
                    response.RemainingAmounnt = remainingAmount;
                }
            }

            return response;

            //if (request.PaymentType == "Cash")
            //{
            //    double discountAmount = request.TotalBill * 0.07;
            //    ResponseAddSurchargeDiscount response = new ResponseAddSurchargeDiscount();
            //    response.CashDiscountAmount = discountAmount;
            //    response.TotalBillWithSurchargeAndDiscount = request.TotalBill - discountAmount;
            //    return response;
            //}
            //else if (request.PaymentType == "Card")
            //{
            //    double surchargeAmount = request.TotalBill * 0.07;
            //    ResponseAddSurchargeDiscount response = new ResponseAddSurchargeDiscount();
            //    response.SurchargeAmount = surchargeAmount;
            //    response.TotalBillWithSurchargeAndDiscount = request.TotalBill + surchargeAmount;
            //    return response;
            //}
            //else if (request.PaymentType == "Both")
            //{
            //    double discountAmount = request.CashAmount * 0.07;
            //    double surchargeAmount = request.CardAmount * 0.07;
            //    ResponseAddSurchargeDiscount response = new ResponseAddSurchargeDiscount();
            //    response.CashDiscountAmount = discountAmount;
            //    response.SurchargeAmount = surchargeAmount;
            //    response.TotalBillWithSurchargeAndDiscount = request.TotalBill + surchargeAmount - discountAmount;
            //    return response;
            //}
            //else
            //{
            //    throw new NotImplementedException("This Payment Type is not implemented yet. Will use Strategy Pattern if need to do it");
            //}
        }
    }
}
