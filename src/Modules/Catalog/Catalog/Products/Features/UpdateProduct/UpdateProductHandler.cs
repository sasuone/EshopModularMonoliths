namespace Catalog.Products.Features.UpdateProduct;

public class UpdateProductHandler(CatalogDbContext dbContext)
	: ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
	public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
	{
		Product? product = await dbContext.Products
			.FindAsync([command.Product.Id], cancellationToken);

		if (product is null)
		{
			throw new ProductNotFoundException(command.Product.Id);
		}

		UpdateProductWithNewValues(product, command.Product);
		
		dbContext.Products.Update(product);
		await dbContext.SaveChangesAsync(cancellationToken);
		
		return new UpdateProductResult(true);
	}

	private void UpdateProductWithNewValues(Product product, ProductDto productDto)
	{
		product.Update(productDto.Name, productDto.Category, productDto.Description, productDto.ImageFile, productDto.Price);
	}
}