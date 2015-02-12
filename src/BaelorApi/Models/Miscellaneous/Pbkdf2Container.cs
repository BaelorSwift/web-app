namespace BaelorApi.Models.Miscellaneous
{
	public class Pbkdf2Container
	{
		/// <summary>
		/// Gets or Sets the number of iterations used to create the <see cref="Hash"/>.
		/// </summary>
		public int Iterations { get; set; }

		/// <summary>
		/// Gets or Sets the salt used to seed the <see cref="Hash"/>.
		/// </summary>
		public string Salt { get; set; }

		/// <summary>
		/// Gets or Sets the generated hash.
		/// </summary>
		public string Hash { get; set; }
	}
}