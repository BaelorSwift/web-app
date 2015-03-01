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
				return _db.Albums
					.Include(a => a.Image)
					.Include(a => a.Songs)
					.ThenInclude(s => s.Lyric).AsEnumerable();
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
			return _db.Albums
					.Include(a => a.Image)
					.Include(a => a.Songs)
					.ThenInclude(s => s.Lyric).FirstOrDefault(a => a.Id == id);
		}

		public Album GetBySlug(string slug)
		{
			return _db.Albums
					.Include(a => a.Image)
					.Include(a => a.Songs)
					.ThenInclude(s => s.Lyric).FirstOrDefault(a => a.Slug == slug);
		}

		public Album Update(Guid id, Album item)
		{
			throw new NotImplementedException();
		}

		public bool TryDelete(Guid id)
		{
			var album = _db.Albums.FirstOrDefault(c => c.Id == id);
			if (album != null)
			{
				_db.Albums.Remove(album);
				return _db.SaveChanges() > 0;
			}
			return false;
		}
	}
}