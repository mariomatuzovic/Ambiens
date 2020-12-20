namespace API.DataTransferObjects
{
  // Dto is a container for moving data between layers and typically does not contain any bussines logic, only simple setters and getters
  public class ProductToReturnDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string PictureUrl { get; set; }
    public string ProductType { get; set; }
    public string ProductBrand { get; set; }
  }
}