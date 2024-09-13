using BS.Services.CustomerFeedbackService.Models.Request;
using BS.Services.CustomerFeedbackService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CustomerFeedbackService
{
    public class CustomerFeedbackService : ICustomerFeedbackService
    {
        public async Task<ResponseAddCustomerFeedback> AddCustomerFeedback(RequestAddCustomerFeedback request, string userId, CancellationToken token)
        {
            ResponseAddCustomerFeedback response = new ResponseAddCustomerFeedback();

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // save to DB
            // send to Hub using gRPC
            
            return response;
        }
    }
}
