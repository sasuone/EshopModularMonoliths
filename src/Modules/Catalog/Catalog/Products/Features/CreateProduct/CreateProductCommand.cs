using Catalog.Contracts.Products.Dtos;
using Shared.Contracts.CQRS;

namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(ProductDto Product)
	: ICommand<CreateProductResult>;