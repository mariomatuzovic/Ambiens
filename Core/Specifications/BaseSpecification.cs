using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
  // BaseSpecification implements ISpecification<T> interface methods
  public class BaseSpecification<T> : ISpecification<T>
  {
    public Expression<Func<T, bool>> Criteria { get; }

    // Includes property is gonna have a list of Include statements
    // We set it by default to empty list
    public List<Expression<Func<T, object>>> Includes { get; } =
      new List<Expression<Func<T, object>>>();

    // We have 2 constructors
    // One in which we can pass in the Criteria
    // If we want to get a product with a specific Id
    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
      Criteria = criteria;
    }

    // We need parameterless constructor to create ProductsWithTypesAndBrandsSpecification class
    public BaseSpecification()
    {
    }

    // Below we created a method that allows us to add Include statements to our Includes list which is a list of Expressions
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
      Includes.Add(includeExpression);
    }
  }
}