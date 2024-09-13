using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CustomerFeedbackService.Models.Request
{
    public class RequestAddCustomerFeedback
    {
        public string CustomerId { get; set; }

        public string CustomerFeedback { get; set; }
    }
}
