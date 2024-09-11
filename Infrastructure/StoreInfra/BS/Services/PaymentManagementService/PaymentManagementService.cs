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
        public async Task<ResponseAddSplitPayments> AddSplitPayments(RequestAddSplitPayments request, string userId, CancellationToken token)
        {
            ResponseAddSplitPayments response = new ResponseAddSplitPayments();
            response.RemainingBalance = request.TotalBill - request.CardPayment - request.CashPayment;
            return response;
        }

        public async Task<ResponseAddSurchargeDiscount> AddSurchargeDiscount(RequestAddSurchargeDiscount request, string userId, CancellationToken token)
        {
            // NEED TO REPLACE SURCHARGE AND DISCOUNT HARDCODED VALUES WITH A NEW ENTITY'S VAR

            if (request.PaymentType == "Cash")
            {
                double discountAmount = request.TotalBill * 0.07;
                ResponseAddSurchargeDiscount response = new ResponseAddSurchargeDiscount();
                response.CashDiscountAmount = discountAmount;
                response.TotalBillWithSurchargeAndDiscount = request.TotalBill - discountAmount;
                return response;
            }
            else if (request.PaymentType == "Card")
            {
                double surchargeAmount = request.TotalBill * 0.07;
                ResponseAddSurchargeDiscount response = new ResponseAddSurchargeDiscount();
                response.SurchargeAmount = surchargeAmount;
                response.TotalBillWithSurchargeAndDiscount = request.TotalBill + surchargeAmount;
                return response;
            }
            else if (request.PaymentType == "Both")
            {
                double discountAmount = request.CashAmount * 0.07;
                double surchargeAmount = request.CardAmount * 0.07;
                ResponseAddSurchargeDiscount response = new ResponseAddSurchargeDiscount();
                response.CashDiscountAmount = discountAmount;
                response.SurchargeAmount = surchargeAmount;
                response.TotalBillWithSurchargeAndDiscount = request.TotalBill + surchargeAmount - discountAmount;
                return response;
            }
            else
            {
                throw new NotImplementedException("This Payment Type is not implemented yet. Will use Strategy Pattern if need to do it");
            }
        }
    }
}
