using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

builder.Services
	.AddCatalogModule(builder.Configuration)
	.AddBasketModule(builder.Configuration)
	.AddOrderingModule(builder.Configuration);

var app = builder.Build();

app.MapCarter();

app.UseCatalogModule()
	.UseBasketModule()
	.UseOrderingModule();

app.UseExceptionHandler(exceptionHandlerApp =>
{
	exceptionHandlerApp.Run(async context =>
	{
		Exception? exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
		if (exception is null)
		{
			return;
		}

		ProblemDetails problemDetails = new ProblemDetails
		{
			Title = exception.Message,
			Status = StatusCodes.Status500InternalServerError,
			Detail = exception.StackTrace
		};
		
		var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
		logger.LogError(exception, exception.Message);
		
		context.Response.StatusCode = StatusCodes.Status500InternalServerError;
		context.Response.ContentType = "application/problem+json";
		
		await context.Response.WriteAsJsonAsync(problemDetails);
	});
});

app.Run();