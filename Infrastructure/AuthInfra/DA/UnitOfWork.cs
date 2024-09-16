


using DA.AppDbContexts;
using DA.Repositories.CommonRepositories;
using DM;
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
        public IGenericRepository<Actions, string> policy => new GenericRepository<Actions, string>(_db);
        public IGenericRepository<RoleAction, string> rolePolicy => new GenericRepository<RoleAction,string>(_db);

        public IGenericRepository<User, string> user => new GenericRepository<User, string>(_db);

        public IGenericRepository<Credential, string> creadential => new GenericRepository<Credential, string>(_db);
        public IGenericRepository<Actions, string> action => new GenericRepository<Actions, string>(_db);

        public IGenericRepository<UserRole, string> userRole => new GenericRepository<UserRole, string>(_db);
        public IGenericRepository<RoleAction, string> roleAction => new GenericRepository<RoleAction, string>(_db);

        public IGenericRepository<RefreshToken, string> refereshToken => new GenericRepository<RefreshToken, string>(_db);

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
