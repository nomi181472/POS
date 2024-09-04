﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CustomerManagementService.Models.Request
{
    public class RequestUpdateCustomer
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Cnic { get; set; }

        public string Billing { get; set; }

        public string Address { get; set; }
    }
}
