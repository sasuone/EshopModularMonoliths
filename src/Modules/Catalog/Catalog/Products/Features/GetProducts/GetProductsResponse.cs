using Catalog.Contracts.Products.Dtos;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsResponse(PaginatedResult<ProductDto> Products);