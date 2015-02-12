using System;

namespace BaelorApi.Extentions
{
	public static class IntegerExtentions
	{
		/// <summary>
		/// Represents the Unix epoch.
		/// </summary>
		public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// Converts this unix timestamp <see cref="int"/> value into a <see cref="DateTime"/>.
		/// </summary>
		public static DateTime FromUnixTime(this int value)
		{
			return Epoch.AddSeconds(value);
		}
	}
}