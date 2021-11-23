using AmazingStore.Infra.Data.Context;
using System.Threading.Tasks;
using AmazingStore.Domain.Shared.UnitOfWork;

namespace AmazingStore.Infra.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields

        private readonly AmazingStoreContext _context;

        #endregion Fields

        #region Constructors

        public UnitOfWork(AmazingStoreContext context)
        {
            _context = context;
        }

        #endregion Constructors

        #region Methods

        public async Task<bool> CommitAsync()
        {
            var teste = await _context.SaveChangesAsync() > 0;

            return teste;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion Methods
    }
}