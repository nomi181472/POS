using Helpers.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.UserService.Models.Response
{
    public class ResponseUserPasswordDetails:ActivityTrackersInResponse
    {

        public string? UserId { get; set; }
    }
}
