using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class ItemGroup : Base<string>
    {
        public string GroupCode { get; set; }
        public string Name { get; set; }

        public ICollection<Items> Items { get; set; }
    }
}
