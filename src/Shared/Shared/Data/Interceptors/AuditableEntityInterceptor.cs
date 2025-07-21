﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.DDD;

namespace Shared.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		UpdateEntities(eventData.Context);
		return base.SavingChanges(eventData, result);
	}

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
	{
		UpdateEntities(eventData.Context);
		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private void UpdateEntities(DbContext? dbContext)
	{
		if (dbContext is null)
		{
			return;
		}

		foreach (EntityEntry<IEntity> entity in dbContext.ChangeTracker.Entries<IEntity>())
		{
			if (entity.State == EntityState.Added)
			{
				entity.Entity.CreatedBy = "daniel";
				entity.Entity.CreatedAt = DateTime.UtcNow;
			}

			if (entity.State == EntityState.Added || entity.State ==  EntityState.Modified || entity.HasChangedOwnedEntities())
			{
				entity.Entity.LastModifiedBy = "daniel";
				entity.Entity.LastModified = DateTime.UtcNow;
			}
		}
	}
}

public static class Extensions
{
	public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
		entry.References.Any(r =>
			r.TargetEntry != null &&
			r.TargetEntry.Metadata.IsOwned() &&
			(r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}