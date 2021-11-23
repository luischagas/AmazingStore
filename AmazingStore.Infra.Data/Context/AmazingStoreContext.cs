using AmazingStore.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace AmazingStore.Infra.Data.Context
{
    public class AmazingStoreContext : DbContext
    {
        #region Protected Methods

        public AmazingStoreContext() { }


        public AmazingStoreContext(DbContextOptions<AmazingStoreContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new ProductMapping());
            modelBuilder.ApplyConfiguration(new OrderMapping());
            modelBuilder.ApplyConfiguration(new OrderProductMapping());

            base.OnModelCreating(modelBuilder);
        }

        #endregion Protected Methods
    }
}