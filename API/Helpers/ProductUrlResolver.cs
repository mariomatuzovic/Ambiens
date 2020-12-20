using API.DataTransferObjects;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
  public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
  {
    private readonly IConfiguration _config;
    public ProductUrlResolver(IConfiguration config)
    {
      _config = config;
    }

    public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
    {
      // This checking below to see if the PictureUrl is empty or not is a bit overcautious because we've configured our db to not accept empty values for this 
      if (!string.IsNullOrEmpty(source.PictureUrl))
      {
        // [] is a property accesser
        // Be careful of typos because Apiurl won't work - C# is a strongly typed language and it protects us quite a lot in our development
        return _config["ApiUrl"] + source.PictureUrl;
      }

      return null;
    }
  }
}