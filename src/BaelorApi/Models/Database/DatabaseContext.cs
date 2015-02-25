using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Entity.Metadata;

namespace BaelorApi.Models.Database
{
	public class DatabaseContext : DbContext
	{
		public DbSet<Album> Albums { get; set; }

		public DbSet<Song> Songs { get; set; }

		public DbSet<Image> Images { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<RateLimit> RateLimits { get; set; }

		protected override void OnConfiguring(DbContextOptions options)
		{

			string connectionString = null;
			if (Startup.Configuration != null)
				Startup.Configuration.TryGet("Data:DefaultConnection:ConnectionString", out connectionString);

			// Must be azure, so we got that access protection layer bro
			if (connectionString == null)
				connectionString = "Server=tcp:vmy5iqiy4g.database.windows.net,1433;Database=baelor-api-sql;User ID=baelor-production@vmy5iqiy4g;Password=nQJuphE2JXaB9N82Uq6f7nxS;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

			options.UseSqlServer(connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Album's have many Songs
			modelBuilder.Entity<Album>()
				.HasMany(a => a.Songs)
				.WithOne(s => s.Album)
				.ForeignKey(s => s.AlbumId);

			// Album's have one Image
			modelBuilder.Entity<Album>()
				.HasOne(a => a.Image);

			// RateLimit's have one User
			modelBuilder.Entity<RateLimit>()
				.HasOne(r => r.User);

			base.OnModelCreating(modelBuilder);
		}

		#region [ Overrides & Audit ]

		public override int SaveChanges()
		{
			UpdateAuditInformation();
			return base.SaveChanges();
		}

		private IEnumerable<EntityEntry> ChangeTrackerEntries
		{
			get { return ChangeTracker.Entries().AsEnumerable(); }
		}

		private void UpdateAuditInformation()
		{
			UpdateAddedEntries();
			UpdateModifiedEntries();
		}

		private void UpdateAddedEntries()
		{
			var addedEntries = ChangeTrackerEntries.Where(e => e.State == EntityState.Added && e.Entity is Audit).Select(e => e.Entity as Audit);
			foreach (var addedEntry in addedEntries)
				addedEntry.UpdatedAt = addedEntry.CreatedAt = DateTime.UtcNow;
		}

		private void UpdateModifiedEntries()
		{
			var modifiedEntries = ChangeTrackerEntries.Where(e => e.State == EntityState.Modified && e.Entity is Audit).Select(e => e.Entity as Audit);
			foreach (var modifiedEntry in modifiedEntries)
				modifiedEntry.UpdatedAt = DateTime.UtcNow;
		}

		#endregion
	}
}
