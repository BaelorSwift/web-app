using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BaelorApi.Extentions
{
	public static class StringExtentions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="phrase"></param>
		/// <param name="maxLength"></param>
		/// <returns></returns>
		public static string ToSlug(this string phrase, int maxLength = 50)
		{
			var str = phrase.ToLower();
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="phrase"></param>
		/// <param name="maxLength"></param>
		/// <returns></returns>
		public static string FromSlug(this string slug)
		{
			// Set first letter to upper
			var slugArr = slug.ToCharArray();
			slugArr[0] = char.ToUpper(slugArr[0]);

			// Gets Indexes of hyphens
			var indexes = new List<int>();
			for (var i = 0; i < slugArr.Length; i++)
				if (slugArr[i] == '-')
					indexes.Add(i + 1);

			// Set all chars after each hypen to upper
			foreach(var index in indexes)
				slugArr[index] = char.ToUpper(slugArr[index]);

			// Swap Hyphens to Spaces
			slug = slugArr.ToString();
			slug.Replace('-', ' ');

			// we gucci
			return slugArr.ToString();
		}
	}
}