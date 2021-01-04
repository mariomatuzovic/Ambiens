using Core.Entities;

namespace Core.Specifications
{
  public class ProductsWithTypesAndColorsSpecification : BaseSpecification<Product>
  {
    public ProductsWithTypesAndColorsSpecification(ProductSpecParams productParams)
      : base(x =>
        (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
        (!productParams.ColorId.HasValue || x.ProductColorId == productParams.ColorId) &&
        (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
      )
    {
      AddInclude(x => x.ProductType);
      AddInclude(x => x.ProductColor);
      AddOrderBy(x => x.Name);
      ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

      if (!string.IsNullOrEmpty(productParams.Sort))
      {
        switch (productParams.Sort)
        {
          case "priceAsc":
            AddOrderBy(p => p.Price);
            break;
          case "priceDesc":
            AddOrderByDescending(p => p.Price);
            break;
          default:
            AddOrderBy(n => n.Name);
            break;
        }
      }
    }

    public ProductsWithTypesAndColorsSpecification(int id) : base(x => x.Id == id)
    {
      AddInclude(x => x.ProductType);
      AddInclude(x => x.ProductColor);
    }
  }
}