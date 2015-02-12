using BaelorApi.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaelorApi.Models.Repositories
{
	public class ImageRepository
		: IImageRepository
	{
		/// <summary>
		/// Gets the <see cref="DatabaseContext"/> used by the repository.
		/// </summary>
		private readonly DatabaseContext _db;

		/// <summary>
		/// Creates a new <see cref="ImageRepository"/>.
		/// </summary>
		/// <param name="db">An initalized <see cref="DatabaseContext"/> used for database connection management.</param>
		public ImageRepository(DatabaseContext db)
		{
			_db = db;
		}

		public IEnumerable<Image> GetAll
		{
			get
			{
				return _db.Images.AsEnumerable();
			}
		}

		public Image Add(Image item)
		{
			_db.Images.Add(item);

			if (_db.SaveChanges() > 0)
				return item;

			return null;
		}

		public Image GetById(Guid id)
		{
			return _db.Images.FirstOrDefault(a => a.Id == id);
		}
		
		public Image Update(Guid id, Image item)
		{
			throw new NotImplementedException();
		}

		public bool TryDelete(Guid id)
		{
			var Image = _db.Images.FirstOrDefault(c => c.Id == id);
			if (Image != null)
			{
				_db.Images.Remove(Image);
				return _db.SaveChanges() > 0;
			}
			return false;
		}
	}
}