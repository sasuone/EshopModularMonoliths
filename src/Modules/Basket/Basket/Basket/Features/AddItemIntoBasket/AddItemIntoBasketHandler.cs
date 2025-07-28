namespace Basket.Basket.Features.AddItemIntoBasket;

public class AddItemIntoBasketHandler(BasketDbContext dbContext)
	: ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
	public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
	{
		ShoppingCart? shoppingCart = await dbContext.ShoppingCarts
			.Include(x => x.Items)
			.SingleOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken);

		if (shoppingCart is null)
		{
			throw new BasketNotFoundException(command.UserName);
		}
		
		shoppingCart.AddItem(
			command.ShoppingCartItem.ProductId,
			command.ShoppingCartItem.Quantity,
			command.ShoppingCartItem.Color,
			command.ShoppingCartItem.Price,
			command.ShoppingCartItem.ProductName
		);
		
		await dbContext.SaveChangesAsync(cancellationToken);

		return new AddItemIntoBasketResult(shoppingCart.Id);
	}
}