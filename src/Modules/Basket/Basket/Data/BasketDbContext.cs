namespace Basket.Data;

public class BasketDbContext : DbContext
{
	public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
	public DbSet<ShoppingCartItem> ShoppingCartItems => Set<ShoppingCartItem>();

	public BasketDbContext(DbContextOptions<BasketDbContext> options)
		: base(options)
	{
		
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("basket");
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}
}