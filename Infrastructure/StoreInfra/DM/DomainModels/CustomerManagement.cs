using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class CustomerManagement : Base<string>
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Cnic { get; set; }

        public string Billing {  get; set; }

        public string Address { get; set; }
        public virtual ICollection<CustomerCart>? CustomerCarts { get; set; }

    }
}
