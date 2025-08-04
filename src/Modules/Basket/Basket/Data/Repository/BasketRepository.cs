namespace Basket.Data.Repository;

public class BasketRepository(BasketDbContext dbContext) 
	: IBasketRepository
{
	public async Task<ShoppingCart> GetBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
	{
		IQueryable<ShoppingCart> query = dbContext.ShoppingCarts
			.Include(x => x.Items)
			.Where(x => x.UserName == userName);

		if (asNoTracking)
		{
			query.AsNoTracking();
		}
		
		ShoppingCart? basket = await query.SingleOrDefaultAsync(cancellationToken);
		
		return basket ?? throw new BasketNotFoundException(userName);
	}

	public async Task<ShoppingCart> CreateBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
	{
		dbContext.ShoppingCarts.Add(shoppingCart);
		
		await dbContext.SaveChangesAsync(cancellationToken);
		
		return shoppingCart;
	}

	public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
	{
		ShoppingCart basket = await GetBasket(userName, false, cancellationToken);
		
		dbContext.ShoppingCarts.Remove(basket);
		await dbContext.SaveChangesAsync(cancellationToken);
		
		return true;
	}

	public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
	{
		return await dbContext.SaveChangesAsync(cancellationToken);
	}
}