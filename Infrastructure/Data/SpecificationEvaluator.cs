using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
  // This class will take a look at what has been provided in our specification - a list of queries and expressions, evaluate it and then create/generate an IQueryable that we will(pass to a ToListAsync function and execute the specification) return so that we can create a list from the list of expression that we've built up in our Speficiration evaluator
  public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
  {
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
    {
      // query is gonna be _context.Products for example
      var query = inputQuery;

      // Evaluate what's inside the specification's criteria
      if (spec.Criteria != null)
      {
        // Get me a product where the product is whatever we've specified as this Criteria
        query = query.Where(spec.Criteria); // p => p.ProductTypeId == typeId
      }

      // Includes evaluation
      // Below we take two Include statements, Aggregate them into an expression and pass them into our query which will be an IQueryable that we then pass to a method so that it can query the database and return a result based on what's contained in this IQueryable
      // current represents the entity that we are passing in
      // include is the expression of our Includes statement
      query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

      return query;
    }
  }
}