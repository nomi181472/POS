using BS.Services.CashManagementService.Models.Request;
using BS.Services.OrderService.Models.Request;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.OrderService.Models
{
    public static class CRTOrderDetails
    {
        public static OrderDetails ToDomain(this RequestAddOrderDetails request)
        {
            return new OrderDetails
            {
                ItemName = request.ItemName,
                Price = request.Price,
                Quantity = request.Quantity,
            };
        }
    }
}
