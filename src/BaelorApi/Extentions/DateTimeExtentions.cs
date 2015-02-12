using System;

namespace BaelorApi.Extentions
{
	public static class DateTimeExtentions
	{
		/// <summary>
		/// Represents the Unix epoch.
		/// </summary>
		public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// Converts this <see cref="DateTime"/> object into a Unix-based timestamp.
		/// </summary>
		public static int ToUnixTime(this DateTime value)
		{
			return Convert.ToInt32((value - Epoch).TotalSeconds);
		}
	}
}