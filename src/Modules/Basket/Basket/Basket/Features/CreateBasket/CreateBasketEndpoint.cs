namespace Basket.Basket.Features.CreateBasket;

public class CreateBasketEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("/basket", async (CreateBasketRequest request, ISender sender) =>
		{
			CreateBasketCommand command = request.Adapt<CreateBasketCommand>();
			
			CreateBasketResult result = await sender.Send(command);
			
			CreateBasketResponse response = result.Adapt<CreateBasketResponse>();
			
			return Results.Created($"/basket/{response.Id}", response);
		})
		.WithName("Create Basket")
		.Produces<CreateBasketResponse>(StatusCodes.Status201Created)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Create Basket")
		.WithDescription("Create Basket");
	}
}