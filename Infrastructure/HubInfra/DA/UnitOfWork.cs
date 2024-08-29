


using DA.AppDbContexts;
using DA.Repositories.CommonRepositories;
using DM.DomainModels;

namespace DA
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
        }

        public IGenericRepository<Location, string> LocationRepo => new GenericRepository<Location, string>(_db);




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
