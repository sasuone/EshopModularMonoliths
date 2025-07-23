namespace Catalog.Products.Features.GetProductsByCategory;

public record GetProductsByCategoryResponse(IEnumerable<ProductDto> Products);