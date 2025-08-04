namespace Basket.Basket.Features.AddItemIntoBasket;

public class AddItemIntoBasketHandler(IBasketRepository repository)
	: ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
	public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
	{
		ShoppingCart shoppingCart = await repository.GetBasket(command.UserName, false, cancellationToken);
		
		shoppingCart.AddItem(
			command.ShoppingCartItem.ProductId,
			command.ShoppingCartItem.Quantity,
			command.ShoppingCartItem.Color,
			command.ShoppingCartItem.Price,
			command.ShoppingCartItem.ProductName
		);
		
		await repository.SaveChangesAsync(shoppingCart.UserName, cancellationToken);

		return new AddItemIntoBasketResult(shoppingCart.Id);
	}
}