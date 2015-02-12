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
		
		protected override void OnConfiguring(DbContextOptions options)
		{
			options.UseSqlServer(Startup.Configuration.Get("Data:DefaultConnection:ConnectionString"));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Album>().OneToMany<Song>(a => a.Songs, s => s.Album).ForeignKey(s => s.AlbumId);
			modelBuilder.Entity<Album>().OneToOne<Image>(a => a.Image).ForeignKey<Album>(a => a.ImageId);
			
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
