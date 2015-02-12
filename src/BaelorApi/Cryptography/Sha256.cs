using System.Security.Cryptography;
using System.Text;

namespace BaelorApi.Cryptography
{
	public static class Sha256
	{
		/// <summary>
		/// Compute a SHA256 hash from a set of data.
		/// </summary>
		/// <param name="data">The data to hash.</param>
		public static string ComputeHash(string data)
		{
			using (var sha = SHA256.Create())
			{
				var computedHash = sha.ComputeHash(Encoding.Unicode.GetBytes(data));
				var hash = string.Empty;
				foreach (byte @byte in computedHash)
					hash += @byte.ToString("x2");
				return hash;
			}
		}

		/// <summary>
		/// Checks if a set of data matches a hash.
		/// </summary>
		/// <param name="data">The data to check.</param>
		/// <param name="hash">The hash to check against.</param>
		/// <returns>A <see cref="Boolean"/> repesentation of the validation.</returns>
		public static bool ValidateHash(string data, string hash)
		{
			var hashOther = ComputeHash(data);
			return hash.Equals(hashOther);
		}
	}
}