WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
	config.ReadFrom.Configuration(context.Configuration));

Assembly[] modules = [
	typeof(CatalogModule).Assembly,
	typeof(BasketModule).Assembly
];

// Common services: Carter, MediatR, FluentValidation
builder.Services
	.AddCarterWithAssemblies(modules)
	.AddMediatRWithAssemblies(modules);

builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// Module services
builder.Services
	.AddCatalogModule(builder.Configuration)
	.AddBasketModule(builder.Configuration)
	.AddOrderingModule(builder.Configuration);

builder.Services
	.AddExceptionHandler<CustomExceptionHandler>();

WebApplication app = builder.Build();

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(_ => { });

app.UseCatalogModule()
	.UseBasketModule()
	.UseOrderingModule();

app.Run();