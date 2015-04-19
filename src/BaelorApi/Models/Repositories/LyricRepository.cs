using BaelorApi.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaelorApi.Models.Repositories
{
	public class LyricRepository
		: ILyricRepository
	{
		/// <summary>
		/// Gets the <see cref="DatabaseContext"/> used by the repository.
		/// </summary>
		private readonly DatabaseContext _db;

		/// <summary>
		/// Creates a new <see cref="LyricRepository"/>.
		/// </summary>
		/// <param name="db">An initalized <see cref="DatabaseContext"/> used for database connection management.</param>
		public LyricRepository(DatabaseContext db)
		{
			_db = db;
		}

		public IEnumerable<Lyric> GetAll
		{
			get
			{
				return _db.Lyrics
					.AsEnumerable();
			}
		}

		public Lyric Add(Lyric item)
		{
			_db.Lyrics.Add(item);

			if (_db.SaveChanges() > 0)
				return item;

			return null;
		}

		public Lyric GetById(Guid id)
		{
			return _db.Lyrics
					.FirstOrDefault(a => a.Id == id);
		}
		
		public Lyric Update(Guid id, Lyric delta)
		{
			var item = _db.Lyrics.FirstOrDefault(r => r.Id == id);
			if (item == null)
				return null;

			item.Content = delta.Content;
			item.TimeCode = delta.TimeCode;
			item.SongId = delta.SongId;

			if (_db.SaveChanges() > 0)
				return delta;

			return item;
		}

		public bool TryDelete(Guid id)
		{
			var Lyric = _db.Lyrics.FirstOrDefault(c => c.Id == id);
			if (Lyric != null)
			{
				_db.Lyrics.Remove(Lyric);
				return _db.SaveChanges() > 0;
			}
			return false;
		}
	}
}