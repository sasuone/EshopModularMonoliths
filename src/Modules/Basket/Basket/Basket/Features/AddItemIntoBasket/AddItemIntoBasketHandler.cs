using Catalog.Contracts.Products.Features.GetProductById;

namespace Basket.Basket.Features.AddItemIntoBasket;

public class AddItemIntoBasketHandler(IBasketRepository repository, ISender sender)
	: ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
	public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
	{
		ShoppingCart shoppingCart = await repository.GetBasket(command.UserName, false, cancellationToken);

		GetProductByIdResult result = await sender.Send(new GetProductByIdQuery(command.ShoppingCartItem.ProductId), cancellationToken);
		
		shoppingCart.AddItem(
			command.ShoppingCartItem.ProductId,
			command.ShoppingCartItem.Quantity,
			command.ShoppingCartItem.Color,
			result.Product.Price,
			result.Product.Name
		);
		
		await repository.SaveChangesAsync(shoppingCart.UserName, cancellationToken);

		return new AddItemIntoBasketResult(shoppingCart.Id);
	}
}