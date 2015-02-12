namespace BaelorApi.Models.Error.Enums
{
	public enum ErrorStatus : uint
	{
		GenericServerError = 0x1069,
		InvalidAlbumSlug = 0x106A,
		InvalidSongSlug = 0x106B,
		SongAlreadyExists = 0x106C,
		AlbumAlreadyExists = 0x106D,
		InvalidImageId = 0x106E
	}
}
