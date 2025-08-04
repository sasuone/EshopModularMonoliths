namespace Basket.Basket.Features.GetBasket;

public class GetBasketHandler(IBasketRepository repository) 
	: IQueryHandler<GetBasketQuery, GetBasketResult>
{
	public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
	{
		ShoppingCart? shoppingCart = await repository.GetBasket(query.UserName, true, cancellationToken);

		ShoppingCartDto shoppingCartDto = shoppingCart.Adapt<ShoppingCartDto>();
		
		return new GetBasketResult(shoppingCartDto);
	}
}