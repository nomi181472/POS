using DM.DomainModels;
using Helpers.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.UserService.Models.Response
{
    public class ResponseUserRefreshTokens:ActivityTrackersInResponse
    {
        public string RefreshTokenId { get; set; }
        public string Token { get; set; }
        public DateTime ExpireyDate { get; set; }
        public bool RevokeAble { get; set; }
        public  string? UserId { get; set; }
    }
}
