﻿namespace Catalog.Products.Features.GetProductById;

public class GetProductByIdHandler(CatalogDbContext dbContext)
	: IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
	public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
	{
		Product?  product = await dbContext.Products
			.AsNoTracking()
			.SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

		if (product is null)
		{
			throw new ProductNotFoundException(query.Id);
		}
		
		ProductDto productDto = product.Adapt<ProductDto>();
		
		return new GetProductByIdResult(productDto);
	}
}