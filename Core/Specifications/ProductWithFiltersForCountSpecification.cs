using Core.Entities;

namespace Core.Specifications
{
  public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
  {
    public ProductWithFiltersForCountSpecification(ProductSpecParams productParams)
      : base(x =>
        (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
        (!productParams.ColorId.HasValue || x.ProductColorId == productParams.ColorId) &&
        (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
      )
    {
    }
  }
}