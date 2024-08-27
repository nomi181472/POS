


using DA.AppDbContexts;
using DA.Models.DomainModels;
using DA.Repositories.CommonRepositories;

namespace DA
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
        }

        public IGenericRepository<Allowance, string> allowanceRepo => new GenericRepository<Allowance, string>(_db);
        public IGenericRepository<WorkingProfile, string> workingProfileRepo => new GenericRepository<WorkingProfile, string>(_db);
        public IGenericRepository<Leave, string> leaveRepo => new GenericRepository<Leave,string>(_db);
        public IGenericRepository<Shift, string> shiftRepo => new GenericRepository<Shift, string>(_db);
        public IGenericRepository<FiscalYear, string> fiscalYearRepo => new GenericRepository<FiscalYear, string>(_db);
        public IGenericRepository<Deduction,string> deductionRepo => new GenericRepository<Deduction,string>(_db);
        public IGenericRepository<ShiftWorkingProfile, string> shiftWorkingProfileRepo => new GenericRepository<ShiftWorkingProfile, string>(_db);

        public void Commit()
        {
            _db.SaveChanges();
        }
        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        public async Task CommitAsync()
        {
            await _db.SaveChangesAsync();
        }

        public virtual void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);

        }

    }
}
