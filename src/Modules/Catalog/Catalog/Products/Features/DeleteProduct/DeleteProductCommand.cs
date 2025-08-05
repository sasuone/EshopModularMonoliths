using Shared.Contracts.CQRS;

namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductCommand(Guid ProductId)
	: ICommand<DeleteProductResult>;