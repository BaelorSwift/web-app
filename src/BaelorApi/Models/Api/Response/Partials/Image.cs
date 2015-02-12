using Newtonsoft.Json;
using System;

namespace BaelorApi.Models.Api.Response.Partials
{
	public class Image
	{
		[JsonProperty("image_id")]
		public Guid ImageId { get; set; }

		public static Image Create(Database.Image image)
		{
			return new Image
			{
				ImageId = image.Id
			};
		}
	}
}