using BS.Services.CashManagementService.Models.Request;
using BS.Services.CashSessionManagementService.Models.Request;
using BS.Services.CashSessionManagementService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CashSessionManagementService
{
    public interface ICashSessionService
    {
        public Task<ResponseAddCashSession> AddCashSession(RequestAddCashSession request, string userId, CancellationToken cancellationToken);
    }
}
