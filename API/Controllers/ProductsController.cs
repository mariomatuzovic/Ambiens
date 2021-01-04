using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using API.DataTransferObjects;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;
using API.Helpers;

namespace API.Controllers
{
  public class ProductsController : BaseApiController
  {
    private readonly IGenericRepository<Product> _productsRepo;
    private readonly IGenericRepository<ProductColor> _productColorRepo;
    private readonly IGenericRepository<ProductType> _productTypeRepo;
    private readonly IMapper _mapper;

    public ProductsController(
      IGenericRepository<Product> productsRepo,
      IGenericRepository<ProductColor> productColorRepo,
      IGenericRepository<ProductType> productTypeRepo,
      IMapper mapper
    )
    {
      _mapper = mapper;
      _productsRepo = productsRepo;
      _productColorRepo = productColorRepo;
      _productTypeRepo = productTypeRepo;
    }

    [HttpGet]
    public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
      [FromQuery] ProductSpecParams productParams
    )
    {
      var spec = new ProductsWithTypesAndColorsSpecification(productParams);

      var countSpec = new ProductWithFiltersForCountSpecification(productParams);

      var totalItems = await _productsRepo.CountAsync(countSpec);

      var products = await _productsRepo.ListAsync(spec);

      var data = _mapper
        .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

      return Ok(new Pagination<ProductToReturnDto>(
        productParams.PageIndex,
        productParams.PageSize,
        totalItems,
        data
      ));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
    {
      var spec = new ProductsWithTypesAndColorsSpecification(id);

      var product = await _productsRepo.GetEntityWithSpec(spec);

      if (product == null)
        return NotFound(new ApiResponse(404));

      return _mapper.Map<Product, ProductToReturnDto>(product);
    }

    [HttpGet("colors")]
    public async Task<ActionResult<IReadOnlyList<ProductColor>>> GetProductColors(int id)
    {
      return Ok(await _productColorRepo.ListAllAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes(int id)
    {
      return Ok(await _productTypeRepo.ListAllAsync());
    }
  }
}