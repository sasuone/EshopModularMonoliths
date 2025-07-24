WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

builder.Services
	.AddCatalogModule(builder.Configuration)
	.AddBasketModule(builder.Configuration)
	.AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

WebApplication app = builder.Build();

app.MapCarter();

app.UseCatalogModule()
	.UseBasketModule()
	.UseOrderingModule();

app.UseExceptionHandler(_ => { });

app.Run();