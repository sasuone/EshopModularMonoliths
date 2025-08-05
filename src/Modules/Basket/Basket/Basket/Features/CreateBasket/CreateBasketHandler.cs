using Catalog.Contracts.Products.Features.GetProductById;

namespace Basket.Basket.Features.CreateBasket;

public class CreateBasketHandler(IBasketRepository repository, ISender sender) 
	: ICommandHandler<CreateBasketCommand,  CreateBasketResult>
{
	public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
	{
		ShoppingCart shoppingCart = await CreateNewBasketAsync(command.ShoppingCart, cancellationToken);
		
		await repository.CreateBasket(shoppingCart, cancellationToken);
		
		return new CreateBasketResult(shoppingCart.Id);
	}

	private async Task<ShoppingCart> CreateNewBasketAsync(ShoppingCartDto shoppingCartDto, CancellationToken cancellationToken)
	{
		ShoppingCart newShoppingCart = ShoppingCart.Create(Guid.NewGuid(), shoppingCartDto.UserName);

		foreach (ShoppingCartItemDto item in shoppingCartDto.Items)
		{
			GetProductByIdResult result = await sender.Send(new  GetProductByIdQuery(item.ProductId), cancellationToken);
			
			newShoppingCart.AddItem(
				item.ProductId,
				item.Quantity,
				item.Color,
				result.Product.Price,
				result.Product.Name
			);
		}
		
		return newShoppingCart;
	}
}