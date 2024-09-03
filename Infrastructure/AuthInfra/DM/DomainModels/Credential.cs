using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Credential:Base<string>
    {
        
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public string? UserId { get; set; }
        public virtual User? User { get; set; }
        public Credential(string pId, string Createdby, DateTime pCreatedDate,string pPasswordSalt,string pPasswordHash,string? pUserId):base(pId, Createdby, pCreatedDate)
        {
            PasswordHash = pPasswordHash;
            PasswordSalt = pPasswordSalt;
            UserId = pUserId;
        }
        public void UpdatePassword(string pPasswordSalt, string pPasswordHash)
        {
            PasswordHash = pPasswordHash;
            PasswordSalt = pPasswordSalt;
        }
    }
}
