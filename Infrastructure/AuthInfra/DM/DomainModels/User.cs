using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class User:Base<string>
    {
        public string Name { get;    set; }
        public string Email { get;  set; }   
       
        public string UserType { get; set; }
        public virtual Credential? Credential { get;  set; }
        public virtual ICollection<UserRole> UserRoles { get;  set; }=new List<UserRole>();
        public User(){}
        public User(string pId, string Createdby, DateTime pCreatedDate, string pName,string pEmail,string pUserType,Credential pCredential,List<UserRole> pUserRoles)         : base(pId, Createdby, pCreatedDate)
        {
            Name=pName;
            Email=pEmail;
            Credential = pCredential;
            UserRoles=pUserRoles;
           
            UserType = pUserType;


        }
        public void UpdateUserType(string pUserType)
        {
            UserType = pUserType;
        }
       
        void UpdateEmail(string PEmail)
        {
            Email = PEmail;
        }
        void UpdateNameAndEmail(string PName,string PEmail)
        {
            Name = PName;
            Email = PEmail;
        }
        void UpdateName(string pName)
        {
            Name = pName;
        }

    }
}
