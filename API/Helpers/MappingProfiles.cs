using API.DataTransferObjects;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
      // AutoMapper is convention based and it is going to look our Entity and Dto, compare the properties and where they match exactly it can deal with it pretty easily but
      // It struggles with productType and productBrand because they have the same name but different types(string vs ProductType) - so he decided that we want our ProductType class output as a string ("productType": "Core.Entities.ProductType")
      // So we need to pass in aditional configuration and tell it explicitly about these 2 properties and what we want it to return these as    
      CreateMap<Product, ProductToReturnDto>()
        // d is short for destination, o for options
        .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
        .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
        .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
    }
  }
}