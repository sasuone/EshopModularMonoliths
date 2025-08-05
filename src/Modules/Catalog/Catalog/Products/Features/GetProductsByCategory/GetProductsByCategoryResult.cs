using Catalog.Contracts.Products.Dtos;

namespace Catalog.Products.Features.GetProductsByCategory;

public record GetProductsByCategoryResult(IEnumerable<ProductDto> Products);