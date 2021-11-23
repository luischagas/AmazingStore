using AmazingStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmazingStore.Infra.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder
                .HasKey(u => u.Id).IsClustered(false);

            builder
                .Property(u => u.CreatedOn)
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(u => u.IsDeleted)
                .IsRequired();

            builder
                .Property(u => u.Name)
                .IsRequired();

            builder
                .Property(u => u.Username)
                .IsRequired();

            builder
                .Property(u => u.Password)
                .IsRequired();

            builder
                .Ignore(p => p.CascadeMode);

            builder
                .Ignore(p => p.ValidationResult);

            builder
                .OwnsOne(c => c.Email)
                .Ignore(e => e.CascadeMode);

            builder
                .OwnsOne(c => c.Email)
                .Ignore(e => e.ValidationResult);

            builder
                .OwnsOne(c => c.Email)
                .Property(e => e.Address)
                .HasColumnName("EmailAddress")
                .HasColumnType("nvarchar(200)")
                .IsRequired();

            builder
                .HasQueryFilter(u => u.IsDeleted == false);
        }

        #endregion Public Methods
    }
}