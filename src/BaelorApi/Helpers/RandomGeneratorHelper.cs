using BaelorApi.Cryptography;
using System;
using System.Security.Cryptography;

namespace BaelorApi.Helpers
{
	public static class RandomGeneratorHelper
	{
		/// <summary>
		/// Generates a set of cryptographly safe bytes of a set length.
		/// </summary>
		/// <param name="length">The number of bytes to generate.</param>
		public static byte[] GenerateRandomBytes(int length)
		{
			var rnJesus = RandomNumberGenerator.Create();
			var randomBytes = new byte[length];
			rnJesus.GetBytes(randomBytes);
			return randomBytes;
		}

		/// <summary>
		/// Generates a Base64 encoded string of cryptographly safe bytes of a set length.
		/// </summary>
		/// <param name="length">The number of bytes to generate.</param>
		public static string GenerateRandomBase64String(int length)
		{
			return Convert.ToBase64String(GenerateRandomBytes(length));
		}

		/// <summary>
		/// Generates a Base32 encoded string of cryptographly safe bytes of a set length.
		/// </summary>
		/// <param name="length">The number of bytes to generate.</param>
		public static string GenerateRandomBase32String(int length)
		{
			return Base32Encoding.ToBase32String(GenerateRandomBytes(length));
		}
	}
}
