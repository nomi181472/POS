


using DA.Repositories.CommonRepositories;
using DM.DomainModels;

namespace DA
{



    public interface IUnitOfWork
    {
        IGenericRepository<CashManagement, string> CashManagementRepo { get; }
        IGenericRepository<OrderDetails, string> OrderDetailsRepo { get; }
        IGenericRepository<Till, string> TillRepo { get; }
        IGenericRepository<CustomerManagement, string> CustomerManagementRepo { get; }
        IGenericRepository<CustomerCart, string> CustomerCartRepo { get; }
        IGenericRepository<CustomerCartItems, string> CustomerCartItemsRepo { get; }
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
        Task CommitAsync();
    }
}
