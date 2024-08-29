using BS.Services.CashManagementService.Models.Request;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CashManagementService.Models
{
    public static class CRTCashManagement
    {
        public static CashManagement ToDomain(this RequestAddCash request)
        {
            return new CashManagement
            {
                Currency = request.Currency,
                Type = request.Type,
                Count = request.Count,
            };
        }
    }
}
