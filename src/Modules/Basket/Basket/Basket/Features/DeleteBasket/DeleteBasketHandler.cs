namespace Basket.Basket.Features.DeleteBasket;

public class DeleteBasketHandler(BasketDbContext dbContext) 
	: ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
	public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
	{
		ShoppingCart? shoppingCart = await dbContext.ShoppingCarts
			.SingleOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken);

		if (shoppingCart is null)
		{
			throw new BasketNotFoundException(command.UserName);
		}
		
		dbContext.Remove(shoppingCart);
		await dbContext.SaveChangesAsync(cancellationToken);
		
		return new DeleteBasketResult(true);
	}
}