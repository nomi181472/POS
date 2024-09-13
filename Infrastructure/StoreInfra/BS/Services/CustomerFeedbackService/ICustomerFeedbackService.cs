using BS.Services.CustomerFeedbackService.Models.Request;
using BS.Services.CustomerFeedbackService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CustomerFeedbackService
{
    public interface ICustomerFeedbackService
    {
        public Task<ResponseAddCustomerFeedback> AddCustomerFeedback(RequestAddCustomerFeedback request, string userId, CancellationToken token);
    }
}
