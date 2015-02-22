using BaelorApi.Models.Database;

namespace BaelorApi.Models.Repositories
{
	public interface IUserRepository
		: IDatabaseRepository<User>
	{
		/// <summary>
		/// Gets an item in the repository with the specified Email Address.
		/// </summary>
		/// <param name="emailAddress">The Email Address of the item to find.</param>
		/// <returns>Returns the item that uses the specified Email Address. Returns null if no item has that Email Address.</returns>
		User GetByEmailAddress(string emailAddress);

		/// <summary>
		/// Gets an item in the repository with the specified Username.
		/// </summary>
		/// <param name="username">The Username of the item to find.</param>
		/// <returns>Returns the item that uses the specified Username. Returns null if no item has that Username.</returns>
		User GetByUsername(string username);

		/// <summary>
		/// Gets an item in the repository with the specified Api Key.
		/// </summary>
		/// <param name="apiKey">The Api Key of the item to find.</param>
		/// <returns>Returns the item that uses the specified Api Key. Returns null if no item has that Api Key.</returns>
		User GetByApiKey(string apiKey);
	}
}