namespace Catalog.Products.Features.DeleteProduct;

public class DeleteProductHandler(CatalogDbContext dbContext)
	: ICommandHandler<DeleteProductCommand,  DeleteProductResult>
{
	public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
	{
		Product?  product = await dbContext.Products
			.FindAsync([command.ProductId], cancellationToken);

		if (product is null)
		{
			throw new Exception($"Product not found: {command.ProductId}");
		}
		
		dbContext.Products.Remove(product);
		await dbContext.SaveChangesAsync(cancellationToken);
		
		return new DeleteProductResult(true);
	}
}