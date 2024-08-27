﻿



using DA.Repositories.CommonRepositories;

namespace DA
{



    public interface IUnitOfWork
    {
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
        Task CommitAsync();
    }
}
