using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
  public class ProductRepository : IProductRepository
  {
    private readonly StoreContext _context;

    public ProductRepository(StoreContext context)
    {
      _context = context;
    }

    public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
    {
      return await _context.ProductBrands.ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
      return await _context.Products
        .Include(p => p.ProductType)
        .Include(p => p.ProductBrand)
        .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
      // For clarifying Specification pattern
      // Suppose that we are passing in 1 as a parameter to GetProductsAsync() and we want to get all products that have that particular Id 
      // var typeId = 1;

      // If we look at the signature of Where extension method then we can see that it returns an IQueryable which is building a list of expressions to query the db against but it does not execute until we execute a ToListAsync
      // We would build up our expression with multiple Where and Include classes, in this case our expression for our IQueryable contains two things to evaluate
      // ToListAsync creates a Generic List from IQueryable asynchronously so we pass an IQueryable to the List to tell it what it is we want it to go to the db and fetch for us
      // We need to recreate this type of behaviour in a generic way and we can also pass in generic expressions to methods
      // var products = _context.Products
      //   .Where(x => x.ProductTypeId == typeId).Include(x => x.ProductType).ToListAsync();

      // Includes property inside BaseSpecification is gonna have a list of these two Include statements below that we can pass to our ToListAsync method
      return await _context.Products
        .Include(p => p.ProductType)
        .Include(p => p.ProductBrand)
        .ToListAsync();
    }

    public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
    {
      return await _context.ProductTypes.ToListAsync();
    }
  }
}