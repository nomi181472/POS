using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.TillManagementService.Response
{
    public class ListAllTillsResponse
    {
        public virtual string? Id {  get; set; }
        public virtual string? Name { get; set; }
        public virtual string? CreatedBy { get; set; }
        public virtual string? CreatedOn { get; set; }
    }
}
