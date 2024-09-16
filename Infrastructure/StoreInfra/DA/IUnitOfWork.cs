


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
        IGenericRepository<Order, string> OrderRepo { get; }
        IGenericRepository<OrderSplitPayments, string> OrderSplitPaymentsRepo { get; }
        IGenericRepository<PaymentMethods, string> PaymentMethodsRepo { get; }
        IGenericRepository<CashDetails, string> CashDetailsRepo { get; }
        IGenericRepository<CashSession, string> CashSessionRepo { get; }

        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
        Task CommitAsync();
    }
}
