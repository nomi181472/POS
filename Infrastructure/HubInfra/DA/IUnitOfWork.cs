


using DA.Models.DomainModels;
using DA.Repositories.CommonRepositories;

namespace DA
{



    public interface IUnitOfWork
    {
        IGenericRepository<Allowance, string> allowanceRepo { get;  }
        IGenericRepository<WorkingProfile, string> workingProfileRepo { get; }
        IGenericRepository<Shift, string> shiftRepo { get; }
        IGenericRepository<FiscalYear, string> fiscalYearRepo { get; }
        IGenericRepository<Leave, string> leaveRepo { get; }
        IGenericRepository<Deduction,string> deductionRepo { get; }
        IGenericRepository<ShiftWorkingProfile, string> shiftWorkingProfileRepo { get; }
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
        Task CommitAsync();
    }
}
