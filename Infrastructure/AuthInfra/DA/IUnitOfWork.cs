using DA.Repositories.CommonRepositories;
using DM.DomainModels;

namespace DA
{



    public interface IUnitOfWork
    {
        IGenericRepository<Role, string> role { get;  }
        IGenericRepository<User, string> user { get;  }
        IGenericRepository<Credential, string> creadential { get;  }
        IGenericRepository<Actions, string> action { get;  }
        IGenericRepository<UserRole, string> userRole { get;  }
        IGenericRepository<RoleAction, string> roleAction { get;  }
      
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
        Task CommitAsync();
    }
}
