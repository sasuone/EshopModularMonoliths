namespace Basket.Basket.Features.AddItemIntoBasket;

public class AddItemIntoBasketEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("/basket/{userName}/items", async ([FromRoute] string userName, [FromBody] AddItemIntoBasketRequest request, ISender sender) =>
		{
			AddItemIntoBasketCommand command = new AddItemIntoBasketCommand(userName, request.ShoppingCartItem);
			
			AddItemIntoBasketResult result = await sender.Send(command);

			AddItemIntoBasketResponse response = result.Adapt<AddItemIntoBasketResponse>();
			
			return Results.Created($"/basket/{response.Id}", response);
		})
		.WithName("Add Item Into Basket")
		.Produces<AddItemIntoBasketResponse>(StatusCodes.Status201Created)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Add Item to Basket")
		.WithDescription("Add Item Into Basket");
	}
}