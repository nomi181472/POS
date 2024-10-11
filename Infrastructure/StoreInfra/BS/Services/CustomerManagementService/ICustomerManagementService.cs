using BS.Services.CashManagementService.Models.Request;
using BS.Services.CustomerManagementService.Models.Request;
using BS.Services.CustomerManagementService.Models.Response;
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
        Task<ResponseAddCustomer> AddCustomer(RequestAddCustomer request, string userId, CancellationToken token);
        Task<bool> UpdateCustomer(RequestUpdateCustomer request, string userId, CancellationToken token);
        Task<List<ResponseListCustomerWithDetails>> ListCustomerWithDetails(string userId, CancellationToken cancellationToken);
        Task<ResponseGetCustomerHistoryById> GetCustomerHistoryById(string customerId, CancellationToken cancellationToken);
    }
}
