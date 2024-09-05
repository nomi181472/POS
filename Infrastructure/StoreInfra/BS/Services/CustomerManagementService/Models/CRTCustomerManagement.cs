using BS.Services.CustomerManagementService.Models.Request;
using BS.Services.OrderService.Models.Request;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CustomerManagementService.Models
{
    public static class CRTCustomerManagement
    {
        public static CustomerManagement ToDomain(this RequestAddCustomer request)
        {
            return new CustomerManagement
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Cnic = request.Cnic,
                Billing = request.Billing,
                Address = request.Address
            };
        }
    }
}
