using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.TillManagementService.Request
{
    public class RequestAddTill
    {
        public virtual string? Name { get; set; }
        public virtual string? Description { get; set; }
        public virtual string? CreatedBy { get; set; }
        public virtual bool? IsActive { get; set; }
    }
}
