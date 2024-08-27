using DA.Repositories.CommonRepositories;
using DM.DomainModels;

namespace DA
{



    public interface IUnitOfWork
    {
        IGenericRepository<Role, string> role { get;  }
        IGenericRepository<Policy, string> policy { get;  }
        IGenericRepository<RolePolicy, string> rolePolicy { get;  }
      
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
        Task CommitAsync();
    }
}
