using Catalog.Contracts.Products.Dtos;
using Shared.Contracts.CQRS;

namespace Catalog.Products.Features.GetProducts;

public class GetProductsHandler(CatalogDbContext dbContext)
	: IQueryHandler<GetProductsQuery, GetProductsResult>
{
	public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
	{
		int pageIndex = query.PaginationRequest.PageIndex;
		int pageSize = query.PaginationRequest.PageSize;

		long totalCount = await dbContext.Products.LongCountAsync(cancellationToken);
		
		List<Product> products = await dbContext.Products
			.AsNoTracking()
			.OrderBy(p => p.Name)
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		List<ProductDto> productDtos = products.Adapt<List<ProductDto>>(); // Mapster
		
		return new GetProductsResult(new PaginatedResult<ProductDto>(pageIndex, pageSize, totalCount, productDtos));
	}
}