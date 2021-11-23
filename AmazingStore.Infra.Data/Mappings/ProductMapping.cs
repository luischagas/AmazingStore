using AmazingStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmazingStore.Infra.Data.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder
                .HasKey(p => p.Id).IsClustered(false);

            builder
                .Property(p => p.CreatedOn)
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(p => p.IsDeleted)
                .IsRequired();

            builder
                .Property(p => p.Name)
                .IsRequired();

            builder
                .Property(p => p.Description)
                .IsRequired();

            builder
                .Ignore(p => p.CascadeMode);

            builder
                .Ignore(p => p.ValidationResult);

            builder
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder
                .HasQueryFilter(p => p.IsDeleted == false);
        }

        #endregion Public Methods
    }
}