using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.InventoryManagementService.Models.Response
{
    public class ResponseGetInventory
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
