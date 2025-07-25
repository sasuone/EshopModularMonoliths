namespace Catalog.Products.Features.GetProducts;

public record GetProductsResult(PaginatedResult<ProductDto> Products);