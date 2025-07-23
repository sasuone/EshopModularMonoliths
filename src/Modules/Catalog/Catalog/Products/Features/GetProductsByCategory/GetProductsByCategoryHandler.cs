namespace Catalog.Products.Features.GetProductsByCategory;

public class GetProductsByCategoryHandler(CatalogDbContext dbContext)
	: IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
	public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
	{
		List<Product> products = await dbContext.Products
			.AsNoTracking()
			.Where(p => p.Category.Contains(query.Category))
			.OrderBy(p => p.Name)
			.ToListAsync(cancellationToken);

		List<ProductDto> productDtos = products.Adapt<List<ProductDto>>(); // Mapster
		
		return new GetProductsByCategoryResult(productDtos);
	}
}