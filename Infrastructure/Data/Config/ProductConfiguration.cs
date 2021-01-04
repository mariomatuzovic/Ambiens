using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
  public class ProductConfiguration : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
      builder.Property(p => p.Id).IsRequired();
      builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
      builder.Property(p => p.Description).IsRequired().HasMaxLength(200);
      builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
      builder.Property(p => p.PictureUrl).IsRequired();
      // Bottom 2 not necessary bcz EF does that already
      builder.HasOne(b => b.ProductColor).WithMany().HasForeignKey(p => p.ProductColorId);
      builder.HasOne(b => b.ProductType).WithMany().HasForeignKey(p => p.ProductTypeId);
    }
  }
}