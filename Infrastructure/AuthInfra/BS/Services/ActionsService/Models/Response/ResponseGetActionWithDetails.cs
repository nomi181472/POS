using Helpers.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.ActionsService.Models.Response
{
    public class ResponseGetActionWithDetails: ActivityTrackersInResponse
    {
        public string? PolicyId { get; set; }    
        public string? ActionName { get; set; }
        public string? Tags { get; set; }
    }
}
