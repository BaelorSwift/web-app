namespace BaelorApi.Models.Error.Enums
{
	public enum ErrorStatus : uint
	{
		GenericServerError = 0x1069,
		InvalidAlbumSlug = 0x106A,
		InvalidSongSlug = 0x106B,
		SongAlreadyExists = 0x106C,
		AlbumAlreadyExists = 0x106D,
		InvalidImageId = 0x106E,
		DataValidationFailed = 0x106F,
		RequestRequiredAuthentication = 0x1070,
		InvalidApiKey = 0x1071,
		RevokedApiKey = 0x1072,
		RateLimitExceeded = 0x1073,
		InsufficientPrivileges = 0x1074,
		SongDoesntContainLyrics = 0x1075
	}
}
