namespace Basket.Basket.Features.GetBasket;

public class GetBasketEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
		{
			GetBasketResult result = await sender.Send(new GetBasketQuery(userName));

			GetBasketResponse response = result.Adapt<GetBasketResponse>();
			
			return Results.Ok(response);
		})
		.WithName("Get Basket")
		.Produces<GetBasketResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Get Basket")
		.WithDescription("Get Basket");
	}
}