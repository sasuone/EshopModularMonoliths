namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdQuery(Guid Id) 
	: IQuery<GetProductByIdResult>;