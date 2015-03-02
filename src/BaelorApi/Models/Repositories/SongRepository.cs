using BaelorApi.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaelorApi.Models.Repositories
{
	public class SongRepository
		: ISongRepository
	{
		/// <summary>
		/// Gets the <see cref="DatabaseContext"/> used by the repository.
		/// </summary>
		private readonly DatabaseContext _db;

		/// <summary>
		/// Creates a new <see cref="SongRepository"/>.
		/// </summary>
		/// <param name="db">An initalized <see cref="DatabaseContext"/> used for database connection management.</param>
		public SongRepository(DatabaseContext db)
		{
			_db = db;
		}

		public IEnumerable<Song> GetAll
		{
			get
			{
				return _db.Songs
					.Include(s => s.Lyric)
					.Include(s => s.Album)
					.Include(s => s.Album.Image).AsEnumerable();
			}
		}

		public Song Add(Song item)
		{
			_db.Songs.Add(item);

			if (_db.SaveChanges() > 0)
				return item;

			return null;
		}

		public Song GetById(Guid id)
		{
			return _db.Songs
					.Include(s => s.Lyric)
					.Include(s => s.Album)
					.Include(s => s.Album.Image).FirstOrDefault(a => a.Id == id);
		}

		public Song GetBySlug(string slug)
		{
			return _db.Songs
					.Include(s => s.Lyric)
					.Include(s => s.Album)
					.Include(s => s.Album.Image).FirstOrDefault(a => a.Slug == slug);
		}

		public Song Update(Guid id, Song delta)
		{
			var item = _db.Songs.FirstOrDefault(r => r.Id == id);
			if (item == null)
				return null;

			item.Index = delta.Index;
			item.LengthSeconds = delta.LengthSeconds;
			item.Producers = delta.Producers;
			item.Writers = delta.Writers;
			item.Title = delta.Title;
			item.Slug = delta.Slug;

			if (_db.SaveChanges() > 0)
				return delta;

			return item;
		}

		public bool TryDelete(Guid id)
		{
			var Song = _db.Songs.FirstOrDefault(c => c.Id == id);
			if (Song != null)
			{
				_db.Songs.Remove(Song);
				return _db.SaveChanges() > 0;
			}
			return false;
		}
	}
}