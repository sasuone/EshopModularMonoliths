namespace Basket.Basket.Features.DeleteBasket;

public class DeleteBasketHandler(IBasketRepository repository) 
	: ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
	public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
	{
		await repository.DeleteBasket(command.UserName, cancellationToken);
		
		return new DeleteBasketResult(true);
	}
}