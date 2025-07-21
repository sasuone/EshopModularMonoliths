namespace Catalog.Data;

public class CatalogDbContext : DbContext
{
	public DbSet<Product> Products => Set<Product>();
	
	public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
		: base(options)
	{
		
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("catalog");
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}
}