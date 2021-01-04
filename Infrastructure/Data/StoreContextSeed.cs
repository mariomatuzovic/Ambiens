using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
  public class StoreContextSeed
  {
    public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
    {
      try
      {
        if (!context.ProductColors.Any())
        {
          var colorsData = File.ReadAllText("../Infrastructure/Data/SeedData/colors.json");

          var colors = JsonSerializer.Deserialize<List<ProductColor>>(colorsData);

          foreach (var item in colors)
          {
            context.ProductColors.Add(item);
          }

          await context.SaveChangesAsync();
        }

        if (!context.ProductTypes.Any())
        {

          var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");

          var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

          foreach (var item in types)
          {
            context.ProductTypes.Add(item);
          }

          await context.SaveChangesAsync();
        }

        if (!context.Products.Any())
        {
          var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");

          var products = JsonSerializer.Deserialize<List<Product>>(productsData);

          foreach (var item in products)
          {
            context.Products.Add(item);
          }

          await context.SaveChangesAsync();
        }
      }
      catch (System.Exception ex)
      {
        var logger = loggerFactory.CreateLogger<StoreContextSeed>();

        logger.LogError(ex.Message);
      }
    }
  }
}