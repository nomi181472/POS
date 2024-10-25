using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class NotificationSeen:Base<string>
    {

        public DateTime OnClickDate { get; set; }
        public string By { get; set; }
        public virtual string? NotificationId { get; set; }
        public virtual Notification? Notification { get; set; } 
        

    }
   
}
