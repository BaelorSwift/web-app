using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using BaelorApi.Models.Miscellaneous;

namespace BaelorApi.Cryptography
{
	public class Pbkdf2
	{
		/// <summary>
		/// The length of salt's to generate.
		/// </summary>
		public const int SaltBytes = 0x25;

		/// <summary>
		/// The length of hash's to generate.
		/// </summary>
		public const int HashBytes = 0x25;

		/// <summary>
		/// Random class used to generate iteration counts.
		/// </summary>
		private static readonly Random IterationRandom = new Random();

		/// <summary>
		/// Creates a salted PBKDF2 hash of the data.
		/// </summary>
		/// <param name="phrase">The data to hash.</param>
		/// <returns>The hash of the data.</returns>
		public static Pbkdf2Container ComputeHash(string data)
		{
			// Generate a random salt
			var rnJesus = RandomNumberGenerator.Create();
			var salt = new byte[SaltBytes];
			rnJesus.GetBytes(salt);

			// Generate a random number of iterations - because why not?
			var iterations = IterationRandom.Next(1000, 1500);

			// Hash the data and encode the parameters
			var hash = GeneratePbkdf2(data, salt, iterations, HashBytes);

			// Return a Container of the Hash details
			return new Pbkdf2Container
			{
				Hash = Convert.ToBase64String(hash),
				Salt = Convert.ToBase64String(salt),
				Iterations = iterations
			};
		}

		/// <summary>
		/// Validates a string of data given a hash of the correct one.
		/// </summary>
		/// <param name="data">The data to check.</param>
		/// <param name="storedHash">A <see cref="Pbkdf2Container"/> containing the elements used to store the Hash.</param>
		/// <returns>True if the data is correct. False otherwise.</returns>
		public static bool ValidateHash(string data, Pbkdf2Container storedHash)
		{
			// Extract the parameters from the hash
			var iterations = storedHash.Iterations;
			var salt = Convert.FromBase64String(storedHash.Salt);
			var hash = Convert.FromBase64String(storedHash.Hash);

			var testHash = GeneratePbkdf2(data, salt, iterations, hash.Length);
			return SlowEquals(hash, testHash);
		}

		/// <summary>
		/// Compares two byte arrays in length-constant time. This comparison
		/// method is used so that data hashes cannot be extracted from
		/// on-line systems using a timing attack and then attacked off-line.
		/// </summary>
		/// <param name="a">The first byte IList.</param>
		/// <param name="b">The second byte IList.</param>
		/// <returns>True if both byte arrays are equal. False otherwise.</returns>
		private static bool SlowEquals(IList<byte> a, IList<byte> b)
		{
			var diff = (uint)a.Count ^ (uint)b.Count;
			for (var i = 0; i < a.Count && i < b.Count; i++)
				diff |= (uint)(a[i] ^ b[i]);
			return diff == 0;
		}

		/// <summary>
		/// Computes the PBKDF2-SHA1 hash of data.
		/// </summary>
		/// <param name="data">The data to hash.</param>
		/// <param name="salt">The salt.</param>
		/// <param name="iterations">The PBKDF2 iteration count.</param>
		/// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
		/// <returns>A hash of the data.</returns>
		private static byte[] GeneratePbkdf2(string data, byte[] salt, int iterations, int outputBytes)
		{
			var pbkdf2 = new Rfc2898DeriveBytes(data, salt)
			{
				IterationCount = iterations
			};
			return pbkdf2.GetBytes(outputBytes);
		}
	}
}
