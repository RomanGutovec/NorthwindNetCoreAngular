using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.ProductId);

            builder.HasIndex(e => e.CategoryId)
                .HasName("CategoryID");

            builder.HasIndex(e => e.ProductName)
                .HasName("ProductName");

            builder.Property(e => e.ProductId).HasColumnName("ProductID");

            builder.Property(e => e.CategoryId).HasColumnName("CategoryID");

            builder.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(e => e.QuantityPerUnit).HasMaxLength(20);

            builder.Property(e => e.ReorderLevel).HasDefaultValueSql("((0))");

            builder.Property(e => e.UnitPrice)
                .HasColumnType("money")
                .HasDefaultValueSql("((0))");

            builder.Property(e => e.UnitsInStock).HasDefaultValueSql("((0))");

            builder.Property(e => e.UnitsOnOrder).HasDefaultValueSql("((0))");

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
