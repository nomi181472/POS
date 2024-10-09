using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.StoreManagementService.Model.Request
{
    public class RequestUpdateStore
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string AdminUser { get; set; }
        public string Address { get; set; }
    }
}
