



using DA.Repositories.CommonRepositories;
using DM.DomainModels;

namespace DA
{



    public interface IUnitOfWork
    {
        IGenericRepository<Location, string> LocationRepo { get; }
        IGenericRepository<Items, string> InventoryItemsRepo { get; }
        IGenericRepository<Store, string> StoreRepo { get; }
        IGenericRepository<StorageLocation, string> StorageLocationRepo { get; }
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
        Task CommitAsync();
    }
}
