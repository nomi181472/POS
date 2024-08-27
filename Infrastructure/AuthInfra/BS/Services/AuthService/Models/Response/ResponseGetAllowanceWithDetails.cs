
using BS.CommonModels.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.AllowanceService.Models.Response
{
    public class ResponseGetAllowanceWithDetails: CompleteTrackingFields
    {
        public string Description { get; set; }
        public string Type { get; set; } 
        public string Tag { get; set; } 
    }
}
