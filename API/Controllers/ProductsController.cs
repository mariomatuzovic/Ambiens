using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {
    private readonly StoreContext _context;
    // We need to inject StoreContext into our ProductsController and by doing that we will get access to the methods in the class
    // When we inject smth into controllers or any class then it is given a lifetime
    // When a request comes in it hits our ProductsController then a new instance of ProductsController is goona be created and when it is created it is going to see what it's dependecies are (in this case the dependency on the StoreContext) and that will create a new instance of the StoreContext that we can work with 
    // ASP.NET Core controls the lifetime of how long this StoreContext is available
    // When we added StoreContext inside Startup class it is given a very specific lifetime
    // If we hoover over AddDbContext we can see that its ServiceLifetime is equal to Scoped(means it is available for the lifetime of the HTTP request) - so this StoreContext will be available for the entirety of that single HTTP request. Once the request is finished the StoreContext is disposed and released from memory
    public ProductsController(StoreContext context)
    {
      _context = context;
    }

    // We are returning an ActionResult from controller below - some kind of HTTP response status like 200, 404, etc
    // Instead of doing it synchronously and waiting for the list to come back we are creating a Task that will pass our request to a delegate which will query the database. It will not wait and block the thread until the Task is completed. Once the request has completed the delegate has completed its Task and notifies our method here to carry on. 
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
      // To list is an LINQ extension method so when we call ToList it will execute a query on the db and return the results and store in the variable products
      var products = await _context.Products.ToListAsync();

      return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
      return await _context.Products.FindAsync(id);
    }
  }
}