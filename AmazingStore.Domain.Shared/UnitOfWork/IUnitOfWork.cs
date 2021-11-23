using System;
using System.Threading.Tasks;

namespace AmazingStore.Domain.Shared.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        #region Public Methods

        Task<bool> CommitAsync();

        #endregion Public Methods

    }
}