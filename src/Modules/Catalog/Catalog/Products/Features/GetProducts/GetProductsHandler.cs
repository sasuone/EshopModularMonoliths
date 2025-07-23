namespace Catalog.Products.Features.GetProducts;

public class GetProductsHandler(CatalogDbContext dbContext)
	: IQueryHandler<GetProductsQuery, GetProductsResult>
{
	public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
	{
		List<Product> products = await dbContext.Products
			.AsNoTracking()
			.OrderBy(p => p.Name)
			.ToListAsync(cancellationToken);

		List<ProductDto> productDtos = products.Adapt<List<ProductDto>>(); // Mapster
		
		return new GetProductsResult(productDtos);
	}
}