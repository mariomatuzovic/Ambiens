using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using API.DataTransferObjects;
using System.Linq;
using AutoMapper;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {
    private readonly IGenericRepository<Product> _productsRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandRepo;
    private readonly IGenericRepository<ProductType> _productTypeRepo;
    private readonly IMapper _mapper;

    // private readonly IProductRepository _repo;

    // public ProductsController(IProductRepository repo)
    // {
    //   _repo = repo;
    // }

    // We replaced the single easy to use repository above with 3 repositories
    public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
    {
      _mapper = mapper;
      _productsRepo = productsRepo;
      _productBrandRepo = productBrandRepo;
      _productTypeRepo = productTypeRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
    {
      var spec = new ProductsWithTypesAndBrandsSpecification();

      // var products = await _repo.GetProductsAsync();
      // var products = await _productsRepo.ListAllAsync();
      var products = await _productsRepo.ListAsync(spec);

      // ToList method below is not running against the db, we have got our products in memory at this point, we are selecting them in memory and then returning it to a list in memory 
      // It would be more efficient to do this in the specification rather then bringing the products back to our controller and then doing this inside our ProductController but this is simpler
      // There is a popular utility that is going to automate the mapping for us between our entities and dtos - AutoMapper
      // return products.Select(product => new ProductToReturnDto
      // {
      //   Id = product.Id,
      //   Name = product.Name,
      //   Description = product.Description,
      //   PictureUrl = product.PictureUrl,
      //   Price = product.Price,
      //   ProductBrand = product.ProductBrand.Name,
      //   ProductType = product.ProductType.Name
      // }).ToList();

      // We changed List to IReadOnlyList above so we wrapped it into Ok
      return Ok(_mapper
        .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
    {
      var spec = new ProductsWithTypesAndBrandsSpecification(id);

      // return await _repo.GetProductByIdAsync(id);
      // return await _productsRepo.GetByIdAsync(id);
      var product = await _productsRepo.GetEntityWithSpec(spec);

      // return new ProductToReturnDto
      // {
      //   Id = product.Id,
      //   Name = product.Name,
      //   Description = product.Description,
      //   PictureUrl = product.PictureUrl,
      //   Price = product.Price,
      //   ProductBrand = product.ProductBrand.Name,
      //   ProductType = product.ProductType.Name
      // };

      return _mapper.Map<Product, ProductToReturnDto>(product);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(int id)
    {
      // return Ok(await _repo.GetProductBrandsAsync());
      return Ok(await _productBrandRepo.ListAllAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes(int id)
    {
      // return Ok(await _repo.GetProductTypesAsync());
      return Ok(await _productTypeRepo.ListAllAsync());
    }
  }
}