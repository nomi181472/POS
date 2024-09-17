using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DM.DomainModels
{
    public class RefreshToken : Base<string>
    {
        public string Token { get; set; }
        public DateTime ExpireyDate { get; set; }
        public bool RevokeAble { get; set; }

        public User? User { get; set; }
        public virtual string? UserId { get; set; }

        public RefreshToken(string pId, string pCreatedby, DateTime pCreatedDate, string pToken, bool pIsrevokable, DateTime pExpiryDate, string pUserId) : base(pId, pCreatedby, pCreatedDate, true)
        {
            Token = pToken;
            ExpireyDate = pExpiryDate;
            RevokeAble = pIsrevokable;
            UserId = pUserId;



        }
        public RefreshToken()
        {

        }
        public bool UpdateRefreshToken(DateTime pExpiryDate, string pToken, string by)
        {

            if (RevokeAble)
            {
                Token = pToken;
                ExpireyDate = pExpiryDate;
                UpdatedBy = by;
                UpdatedDate = DateTime.UtcNow;
                return true;
            }

            throw new Exception("token is non revokable");
        }
        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpireyDate;
        }
        public bool IsTokenMatch(string provided)
        {
            return Token.Equals(provided);
        }

    }



}
