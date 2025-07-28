namespace Basket.Basket.Features.GetBasket;

public class GetBasketHandler(BasketDbContext dbContext) 
	: IQueryHandler<GetBasketQuery, GetBasketResult>
{
	public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
	{
		ShoppingCart? shoppingCart = await dbContext.ShoppingCarts
			.AsNoTracking()
			.Include(x => x.Items)
			.SingleOrDefaultAsync(x => x.UserName == query.UserName, cancellationToken);

		if (shoppingCart is null)
		{
			throw new BasketNotFoundException(query.UserName);
		}

		ShoppingCartDto shoppingCartDto = shoppingCart.Adapt<ShoppingCartDto>();
		
		return new GetBasketResult(shoppingCartDto);
	}
}