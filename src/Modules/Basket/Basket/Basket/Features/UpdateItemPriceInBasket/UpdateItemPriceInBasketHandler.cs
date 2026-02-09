namespace Basket.Basket.Features.UpdateItemPriceInBasket;

public class UpdateItemPriceInBasketHandler(BasketDbContext dbContext)
	: ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
{
	public async Task<UpdateItemPriceInBasketResult> Handle(UpdateItemPriceInBasketCommand command, CancellationToken cancellationToken)
	{
		List<ShoppingCartItem> itemsToUpdate = await dbContext.ShoppingCartItems.Where(x => x.ProductId == command.ProductId).ToListAsync(cancellationToken);

		if (!itemsToUpdate.Any())
		{
			return new UpdateItemPriceInBasketResult(false);
		}
		
		foreach (ShoppingCartItem item in itemsToUpdate)
		{
			item.UpdatePrice(command.Price);
		}
		
		await dbContext.SaveChangesAsync(cancellationToken);
		
		return new UpdateItemPriceInBasketResult(true);
	}
}