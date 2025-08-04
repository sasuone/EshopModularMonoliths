namespace Basket.Basket.Features.RemoveItemFromBasket;

public class RemoveItemFromBasketHandler(IBasketRepository repository)
	: ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
	public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
	{
		ShoppingCart shoppingCart = await repository.GetBasket(command.UserName, false, cancellationToken);
		
		shoppingCart.RemoveItem(command.ProductId);
		
		await repository.SaveChangesAsync(shoppingCart.UserName, cancellationToken);
		
		return new RemoveItemFromBasketResult(shoppingCart.Id);
	}
}