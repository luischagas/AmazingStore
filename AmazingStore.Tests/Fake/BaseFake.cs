using System;
using AmazingStore.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AmazingStore.Tests.Fake
{
    public abstract class BaseFake
    {
        #region Public Methods

        public AmazingStoreContext CreateInMemoryDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AmazingStoreContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            return new AmazingStoreContext(optionsBuilder.Options);
        }

        public AmazingStoreContext CreateInMemoryDatabaseOptional()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AmazingStoreContext>();
            optionsBuilder.UseInMemoryDatabase("InMemoryBase");
            return new AmazingStoreContext(optionsBuilder.Options);
        }

        #endregion Public Methods
    }
}