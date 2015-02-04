namespace BaelorApi.Models.Error.Enums
{
	public enum ErrorStatus : uint
	{
		GenericServerError = 0x1069,
		InvalidAlbumSlug = 0x106A,
		InvalidSongSlug = 0x106B
	}
}