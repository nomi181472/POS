using BS.Services.CashManagementService.Models.Request;
using BS.Services.CustomerManagementService.Models.Request;
using BS.Services.OrderService.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CustomerManagementService
{
    public interface ICustomerManagementService
    {
        public Task<bool> AddCustomer(RequestAddCustomer request, string userId, CancellationToken token);

        public Task<bool> UpdateCustomer(RequestUpdateCustomer request, string userId, CancellationToken token);
    }
}
