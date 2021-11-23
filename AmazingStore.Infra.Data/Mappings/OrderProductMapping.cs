using AmazingStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmazingStore.Infra.Data.Mappings
{
    public class OrderProductMapping : IEntityTypeConfiguration<OrderProduct>
    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.ToTable("OrderProducts");

            builder
                .HasKey(op => op.Id).IsClustered(false);

            builder
                .Property(op => op.CreatedOn)
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(op => op.IsDeleted)
                .IsRequired();

            builder
                .Property(p => p.CurrentPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder
                .Property(op => op.Quantity)
                .IsRequired();

            builder
                .Ignore(op => op.CascadeMode);

            builder
                .Ignore(op => op.ValidationResult);

            builder
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            builder
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId);

            builder
                .HasQueryFilter(op => op.IsDeleted == false);
        }

        #endregion Public Methods
    }
}