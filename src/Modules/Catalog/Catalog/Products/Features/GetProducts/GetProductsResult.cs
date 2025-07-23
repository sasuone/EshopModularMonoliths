namespace Catalog.Products.Features.GetProducts;

public record GetProductsResult(IEnumerable<ProductDto> Products);