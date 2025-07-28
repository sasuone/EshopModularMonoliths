namespace Basket.Basket.Features.CreateBasket;

public class CreateBasketHandler(BasketDbContext dbContext) 
	: ICommandHandler<CreateBasketCommand,  CreateBasketResult>
{
	public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
	{
		ShoppingCart shoppingCart = CreateNewBasket(command.ShoppingCart);
		
		dbContext.ShoppingCarts.Add(shoppingCart);
		await dbContext.SaveChangesAsync(cancellationToken);
		
		return new CreateBasketResult(shoppingCart.Id);
	}

	private ShoppingCart CreateNewBasket(ShoppingCartDto shoppingCartDto)
	{
		ShoppingCart newShoppingCart = ShoppingCart.Create(Guid.NewGuid(), shoppingCartDto.UserName);
		
		shoppingCartDto.Items.ForEach(item =>
			newShoppingCart.AddItem(
				item.ProductId, 
				item.Quantity, 
				item.Color, 
				item.Price, 
				item.ProductName
			)
		);
		
		return newShoppingCart;
	}
}