



using DA.Repositories.CommonRepositories;
using DM.DomainModels;

namespace DA
{



    public interface IUnitOfWork
    {
        IGenericRepository<Location, string> LocationRepo { get; }
        IGenericRepository<InventoryItems, string> InventoryItemsRepo { get; }
        IGenericRepository<InventoryGroups, string> InventoryGroupsRepo { get; }
        IGenericRepository<InventoryCategories, string> InventoryCategoriesRepo { get; }
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
        Task CommitAsync();
    }
}
