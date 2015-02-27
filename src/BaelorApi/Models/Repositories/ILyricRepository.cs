using BaelorApi.Models.Database;

namespace BaelorApi.Models.Repositories
{
	public interface ILyricRepository
		: IDatabaseRepository<Lyric>
	{
		/// <summary>
		/// Gets an item in the repository with the specified Slug.
		/// </summary>
		/// <param name="slug">The Slug of the item to find.</param>
		/// <returns>Returns th item that uses the specified Slug. Returns null if no item has that Slug.</returns>
		Lyric GetBySlug(string slug);
	}
}