namespace Basket.Basket.Features.RemoveItemFromBasket;

public class RemoveItemFromBasketEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapDelete("/basket/{userName}/items/{productId}", async ([FromRoute] string userName, [FromRoute] Guid productId, ISender sender) =>
		{
			RemoveItemFromBasketCommand command = new RemoveItemFromBasketCommand(userName, productId);
			
			RemoveItemFromBasketResult result = await sender.Send(command);

			RemoveItemFromBasketResponse response = result.Adapt<RemoveItemFromBasketResponse>();
			
			return Results.Ok(response);
		})
		.WithName("Remove Item From Basket")
		.Produces<RemoveItemFromBasketResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Remove Item From Basket")
		.WithDescription("Remove Item From Basket");
	}
}