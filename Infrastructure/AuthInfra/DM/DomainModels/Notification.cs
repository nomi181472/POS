using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Notification : Base<string>
    {

        public string Title { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public DateTime At { get; set; }

        public string TargetNamespace { get; set; }
        public string SendToUserType { get; set; }
        public string Tag { get; set; }
        
        public virtual string  UserId { get; set; }
        public virtual User User { get; set; }
        
        public  NotificationSeen? IsSeen { get; set; }

    }
    
}
