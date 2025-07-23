namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(ProductDto Product)
	: ICommand<CreateProductResult>;