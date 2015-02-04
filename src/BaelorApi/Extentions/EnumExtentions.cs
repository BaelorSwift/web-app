using System.Collections.Generic;
using BaelorApi.Models.Error.Enums;

namespace BaelorApi.Extentions
{
	public static class EnumExtentions
	{
		/// <summary>
		/// Dictionary of friendly error status messages.
		/// </summary>
		private static readonly Dictionary<ErrorStatus, string> ErrorStatusFriendly = new Dictionary<ErrorStatus, string>
		{
			// Server Stuff
			{ ErrorStatus.GenericServerError, "generic_server_error" },

			// Album Stuff
			{ ErrorStatus.InvalidAlbumSlug, "invalid_album_slug" },

			// Song Stuff
			{ ErrorStatus.InvalidSongSlug, "invalid_song_slug" }
		};

		/// <summary>
		/// Get a friendly error message from a <see cref="ErrorStatus"/>.
		/// </summary>
		/// <param name="value">The <see cref="ErrorStatus"/> in question.</param>
		public static string GetDisplayDescription(this ErrorStatus value)
		{
			return ErrorStatusFriendly.ContainsKey(value)
				? ErrorStatusFriendly[value]
				: value.ToString();
		}
	}
}