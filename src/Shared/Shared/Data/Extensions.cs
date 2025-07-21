using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Seed;

namespace Shared.Data;

public static class Extensions
{
	public static IApplicationBuilder UseMigration<T>(this IApplicationBuilder app) where T : DbContext
	{
		MigrateDatabaseAsync<T>(app.ApplicationServices).GetAwaiter().GetResult();
		SeedDataAsync(app.ApplicationServices).GetAwaiter().GetResult();
		
		return app;
	}
	
	private static async Task MigrateDatabaseAsync<T>(IServiceProvider serviceProvider) where T : DbContext
	{
		using IServiceScope serviceScope = serviceProvider.CreateScope();
		T dbContext = serviceScope.ServiceProvider.GetRequiredService<T>();
		await dbContext.Database.MigrateAsync();
	}

	private static async Task SeedDataAsync(IServiceProvider serviceProvider)
	{
		using IServiceScope serviceScope = serviceProvider.CreateScope();
		IEnumerable<IDataSeeder> seeders = serviceScope.ServiceProvider.GetServices<IDataSeeder>();
		
		foreach (IDataSeeder seeder in seeders)
		{
			await seeder.SeedAllAsync();
		}
	}
}