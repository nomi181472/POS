﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.OrderService.Models.Request
{
    public class RequestAddOrderDetails
    {
        public string ItemName { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }
    }
}
