using System;
using System.Collections.Generic;

namespace BaelorApi.Models.Repositories
{
	public interface IRepository<T>
		where T : class
	{
		/// <summary>
		/// Gets all the items in the Repository.
		/// </summary>
		IEnumerable<T> GetAll { get; }

		/// <summary>
		/// Gets an item in the repository with the specified Id.
		/// </summary>
		/// <param name="id">The Id of the item to find.</param>
		/// <returns>Returns th item that uses the specified Id. Returns null if no item has that Id.</returns>
		T GetById(Guid id);

		/// <summary>
		/// Add a new item to the repository.
		/// </summary>
		/// <param name="item">The item to add to the repository.</param>
		/// <returns>Returns the item that was created. Returns null if there was an error inserting the item.</returns>
		T Add(T item);

		/// <summary>
		/// Updates an item with a specified Id.
		/// </summary>
		/// <param name="id">The Id of the item to update.</param>
		/// <param name="update">The updated item data.</param>
		/// <returns>The updated item, from the repository.</returns>
		T Update(Guid id, T item);

		/// <summary>
		/// Deletes an item from the repository by it's Id.
		/// </summary>
		/// <param name="id">The Id of the item to delete.</param>
		/// <returns>A <see cref="Boolean"/> representation of if the deletion was successful.</returns>
		bool TryDelete(Guid id);
	}
}