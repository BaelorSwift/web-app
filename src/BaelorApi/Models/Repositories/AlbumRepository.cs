using BaelorApi.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaelorApi.Models.Repositories
{
	public class AlbumRepository
		: IAlbumRepository
	{
		/// <summary>
		/// Gets the <see cref="DatabaseContext"/> used by the repository.
		/// </summary>
		private readonly DatabaseContext _db;

		/// <summary>
		/// Creates a new <see cref="AlbumRepository"/>.
		/// </summary>
		/// <param name="db">An initalized <see cref="DatabaseContext"/> used for database connection management.</param>
		public AlbumRepository(DatabaseContext db)
		{
			_db = db;
		}

		public IEnumerable<Album> GetAll
		{
			get
			{
				return _db.Albums.AsEnumerable();
			}
		}

		public Album Add(Album item)
		{
			_db.Albums.Add(item);

			if (_db.SaveChanges() > 0)
				return item;

			return null;
		}

		public Album GetById(Guid id)
		{
			throw new NotImplementedException();
		}

		public Album GetBySlug(string slug)
		{
			throw new NotImplementedException();
		}

		public Album Update(Guid id, Album item)
		{
			throw new NotImplementedException();
		}

		public bool TryDelete(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}