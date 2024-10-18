using BS.CustomExceptions.Common;
using DA;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.AdminDashboardService
{
    public class AdminDashboardService : IAdminDashboardService
    {
        IUnitOfWork _unitOfWork;
        IConfiguration _configuration;
        public AdminDashboardService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }



        public async Task<int> GetNewUsersByMonth(int month, CancellationToken cancellationToken)
        {
            #region Request Validations
            if (month < 1 || month > 12)
            {
                throw new ArgumentException("Invalid month. Please provide a valid month in MM format.");
            }
            int currentYear = DateTime.Now.Year;
            var startDate = new DateTime(currentYear, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            #endregion Request Validations

            var userResults = await _unitOfWork.user.GetAsync(cancellationToken, x => x.CreatedDate >= startDate && x.CreatedDate <= endDate);
            if (userResults.Data == null)
            {
                throw new RecordNotFoundException("No user records found for month");
            }
            return userResults.Data.Count();
        }



        public async Task<int> GetReportedBugsByMonth(int month, CancellationToken cancellationToken)
        {
            #region Request Validations
            if (month < 1 || month > 12)
            {
                throw new ArgumentException("Invalid month. Please provide a valid month in MM format.");
            }
            int currentYear = DateTime.Now.Year;
            var startDate = new DateTime(currentYear, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            #endregion Request Validations

            //var bugResults = await _unitOfWork.user.GetAsync(cancellationToken, x => x.CreatedDate>=startDate && x.CreatedDate<=endDate);
            //if(bugResults.Data == null)
            //{
            //    throw new RecordNotFoundException("No bug records found for month");
            //}
            //return bugResults.Data.Count();
            throw new Exception("Not implemented yet");
        }



    }
}
