using BaelorApi.Models.Error.Enums;
using Newtonsoft.Json;

namespace BaelorApi.Models.Api.Error
{
	public class ErrorBase
	{
		public ErrorBase() { }

		public ErrorBase(ErrorStatus statusCode)
		{
			StatusCode = statusCode;
			Description = StatusCode.ToString(); // TODO: impliment error_desc functions
		}

		[JsonProperty("status_code")]
		public ErrorStatus StatusCode { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("details")]
		public object Details { get; set; }
	}
}