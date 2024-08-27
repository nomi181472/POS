


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

        public IGenericRepository<Role, string> role => new GenericRepository<Role, string>(_db);
        public IGenericRepository<Policy, string> policy => new GenericRepository<Policy, string>(_db);
        public IGenericRepository<RolePolicy, string> rolePolicy => new GenericRepository<RolePolicy,string>(_db);
      

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
