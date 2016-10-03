using System.Text.RegularExpressions;

namespace Baelor.Extensions
{
	public static class StringExtensions
	{
		public static string ToSlug(this string phrase, bool convertFromUpperCamelCase = false, int maxLength = 50)
		{
			var str = phrase;

			if (convertFromUpperCamelCase)
			{
				// convert to upper-camel-case to hyphenated-case
				str = Regex.Replace(str, @"(\B[A-Z])", "-$1");
			}

			// make string lowercase
			str = str.ToLowerInvariant();

			// invalid chars, make into spaces
			str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

			// convert multiple spaces/hyphens into one space
			str = Regex.Replace(str, @"[\s-]+", " ").Trim();

			// cut and trim it
			str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();

			// hyphens
			str = Regex.Replace(str, @"\s", "-");

			return str;
		}
	}
}
