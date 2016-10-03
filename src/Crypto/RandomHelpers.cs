using System.Security.Cryptography;
using System.Text;

namespace Baelor.Crypto
{
	public static class RandomHelpers
	{
		public static string GetUniqueKey(int maxSize)
		{
			var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
			byte[] data = new byte[1];
			using (var crypto = RandomNumberGenerator.Create())
			{
				crypto.GetBytes(data);
				data = new byte[maxSize];
				crypto.GetBytes(data);
			}

			var result = new StringBuilder(maxSize);
			foreach (byte b in data)
				result.Append(chars[b % (chars.Length)]);

			return result.ToString();
		}
	}
}
