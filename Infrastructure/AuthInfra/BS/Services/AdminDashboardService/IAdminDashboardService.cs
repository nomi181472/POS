using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.AdminDashboardService
{
    public interface IAdminDashboardService
    {
        Task<int> GetNewUsersByMonth(int month, CancellationToken cancellationToken);
        Task<int> GetReportedBugsByMonth(int month, CancellationToken cancellationToken);
    }
}
