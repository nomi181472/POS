using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CustomerManagementService.Models.Response
{
    public class ResponseGetCustomerHistoryById
    {
        public string CustomerName { get; set; }
        public List<CustomerHistoryDTO> CustomerHistory {  get; set; }
    }

    public class CustomerHistoryDTO
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string GroupCodeName { get; set; }
    }
}
