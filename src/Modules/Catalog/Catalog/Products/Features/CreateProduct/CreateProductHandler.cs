using Catalog.Contracts.Products.Dtos;
using Shared.Contracts.CQRS;

namespace Catalog.Products.Features.CreateProduct;

public class CreateProductHandler(CatalogDbContext dbContext)
	: ICommandHandler<CreateProductCommand, CreateProductResult>
{
	public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
	{
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