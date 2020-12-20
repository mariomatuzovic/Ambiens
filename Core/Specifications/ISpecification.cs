using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
  public interface ISpecification<T>
  {
    // We will replace whats going on below with actual expression
    // Below we created GENERIC methods with Expression generic class which takes a function Func which takes a type and what is returning(boolean value)
    // The first one is Criteria - what's the criteria of the thing that we are going to get and it could be where the product has a typeId of 1 in ISpecification dummy code
    Expression<Func<T, bool>> Criteria { get; }
    // We also have a List which takes a Generic Expression of Func Type T and returns an object(most generic thing we can use in c#)
    // This is gonna be for our Includes operations
    List<Expression<Func<T, object>>> Includes { get; }
  }
}