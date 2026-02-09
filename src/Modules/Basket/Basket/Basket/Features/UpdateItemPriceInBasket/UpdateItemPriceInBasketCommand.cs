namespace Basket.Basket.Features.UpdateItemPriceInBasket;

public record UpdateItemPriceInBasketCommand(Guid ProductId, decimal Price)
	: ICommand<UpdateItemPriceInBasketResult>
{
	
}