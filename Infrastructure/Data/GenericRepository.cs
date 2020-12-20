using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
  public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
  {
    private readonly StoreContext _context;

    // We need to bring in the constructor because we need to bring in the StoreContext
    public GenericRepository(StoreContext context)
    {
      _context = context;
    }

    public async Task<T> GetByIdAsync(int id)
    {
      // Set<T> allows us to save instances of the type of entity for which a set should be returned
      // This T gets replaced with whatever thing we pass in as the type when we use this particular repository
      return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
      return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
    {
      return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
      return await ApplySpecification(spec).ToListAsync();
    }

    // Method below will allow us to apply our specifications
    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
      // Static method we created inside SpecificationEvaluator GetQuery will take an IQueryable of type T, which will be our inputQuery, as well as our specification  
      // The type T which we passed in as argument will be replaced with Product for example and it is going to be converted into a Queryable 
      // _context.Set<T>().AsQueryable() is gonna be _context.Products
      return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }
  }
}