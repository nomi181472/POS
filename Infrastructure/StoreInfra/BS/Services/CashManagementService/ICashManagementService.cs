using BS.Services.CashManagementService.Models.Request;
using BS.Services.CashManagementService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.CashManagementService
{
    public interface ICashManagementService
    {
        public Task<bool> AddCash(RequestAddCash request, string userId, CancellationToken token);

        public Task<List<ResponseListCash>> ListCashWithDetails (string  userId, CancellationToken token);
    }
}
