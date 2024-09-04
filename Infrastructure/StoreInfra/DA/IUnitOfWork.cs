


using DA.Repositories.CommonRepositories;
using DM.DomainModels;

namespace DA
{



    public interface IUnitOfWork
    {
        IGenericRepository<CashManagement, string> CashManagementRepo { get; }
        IGenericRepository<OrderDetails, string> OrderDetailsRepo { get; }
        IGenericRepository<CustomerManagement, string> CustomerManagementRepo { get; }
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
        Task CommitAsync();
    }
}
