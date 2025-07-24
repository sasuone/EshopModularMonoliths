﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviors;
using Shared.Data;
using Shared.Data.Interceptors;
using Shared.Data.Seed;

namespace Catalog;

public static class CatalogModule
{
	public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			config.AddOpenBehavior(typeof(ValidationBehavior<,>));
		});
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		
		string? connectionString = configuration.GetConnectionString("Database");

		services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
		services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
		
		services.AddDbContext<CatalogDbContext>((sp, options) => {
			options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
			options.UseNpgsql(connectionString);
		});
		
		services.AddScoped<IDataSeeder, CatalogDataSeeder>();
		
		return services;
	}

	public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
	{
		app.UseMigration<CatalogDbContext>();
		
		return app;
	}
}