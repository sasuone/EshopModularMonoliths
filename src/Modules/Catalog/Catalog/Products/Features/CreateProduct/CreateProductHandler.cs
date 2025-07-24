using FluentValidation.Results;

namespace Catalog.Products.Features.CreateProduct;

public class CreateProductHandler(
	CatalogDbContext dbContext, 
	ILogger<CreateProductHandler>  logger) 
	: ICommandHandler<CreateProductCommand, CreateProductResult>
{
	public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
	{
		// Logging
		logger.LogInformation("CreateProductHandler.Handle called with {@Command}", command);
		
		// Logic
		Product product = CreateNewProduct(command.Product);
		dbContext.Products.Add(product);
		await dbContext.SaveChangesAsync(cancellationToken);
		
		return new CreateProductResult(product.Id);
	}

	private Product CreateNewProduct(ProductDto productDto)
	{
		Product product = Product.Create(
			Guid.NewGuid(), 
			productDto.Name,
			productDto.Category,
			productDto.Description,
			productDto.ImageFile,
			productDto.Price
		);
		
		return product;
	}
}