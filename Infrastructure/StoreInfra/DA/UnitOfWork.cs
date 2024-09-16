


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

        public IGenericRepository<CashManagement, string> CashManagementRepo => new GenericRepository<CashManagement, string>(_db);
        public IGenericRepository<OrderDetails, string> OrderDetailsRepo => new GenericRepository<OrderDetails, string>(_db);
        public IGenericRepository<Till, string> TillRepo => new GenericRepository<Till, string>(_db);
        public IGenericRepository<CustomerManagement, string> CustomerManagementRepo => new GenericRepository<CustomerManagement, string>(_db);
        public IGenericRepository<CustomerCart, string> CustomerCartRepo => new GenericRepository<CustomerCart, string>(_db);
        public IGenericRepository<CustomerCartItems, string> CustomerCartItemsRepo => new GenericRepository<CustomerCartItems, string>(_db);
        public IGenericRepository<Order, string> OrderRepo => new GenericRepository<Order, string>(_db);
        public IGenericRepository<OrderSplitPayments, string> OrderSplitPaymentsRepo => new GenericRepository<OrderSplitPayments, string>(_db);
        public IGenericRepository<PaymentMethods, string> PaymentMethodsRepo => new GenericRepository<PaymentMethods, string>(_db);
        public IGenericRepository<CashDetails, string> CashDetailsRepo => new GenericRepository<CashDetails, string>(_db);
        public IGenericRepository<CashSession, string> CashSessionRepo => new GenericRepository<CashSession, string>(_db);

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
