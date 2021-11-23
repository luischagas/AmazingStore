using AmazingStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmazingStore.Infra.Data.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder
                .HasKey(o => o.Id).IsClustered(false);

            builder
                .Property(o => o.CreatedOn)
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(o => o.IsDeleted)
                .IsRequired();

            builder
                .Property(o => o.Status)
                .IsRequired();

            builder
                .Ignore(o => o.CascadeMode);

            builder
                .Ignore(o => o.ValidationResult);

            builder
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            builder
                .HasQueryFilter(o => o.IsDeleted == false);
        }

        #endregion Public Methods
    }
}