using BS.Services.CashManagementService.Models.Request;
using BS.Services.CashSessionManagementService.Models.Request;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CashSessionManagementService.Models
{
    public static class CRTCashSession
    {
        public static CashSession ToDomain(this RequestAddCashSession request)
        {
            return new CashSession
            {
                //Id = Guid.NewGuid().ToString(),
                TillId = request.TillId,
                UserId = request.UserId,
                TotalAmount = request.TotalAmount,
            };
        }
    }
}
