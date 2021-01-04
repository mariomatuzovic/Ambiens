using API.DataTransferObjects;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
      CreateMap<Product, ProductToReturnDto>()
        .ForMember(d => d.ProductColor, o => o.MapFrom(s => s.ProductColor.Name))
        .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
        .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
    }
  }
}