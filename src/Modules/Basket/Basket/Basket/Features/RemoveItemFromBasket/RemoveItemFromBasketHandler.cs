namespace Basket.Basket.Features.RemoveItemFromBasket;

public class RemoveItemFromBasketHandler(BasketDbContext dbContext)
	: ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
	public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
	{
		ShoppingCart? shoppingCart = await dbContext.ShoppingCarts
			.Include(x => x.Items)
			.SingleOrDefaultAsync(s => s.UserName == command.UserName, cancellationToken);

		if (shoppingCart is null)
		{
			throw new BasketNotFoundException(command.UserName);
		}
		
		shoppingCart.RemoveItem(command.ProductId);
		
		await dbContext.SaveChangesAsync(cancellationToken);
		
		return new RemoveItemFromBasketResult(shoppingCart.Id);
	}
}