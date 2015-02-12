using BaelorApi.Models.Api.Error;
using Newtonsoft.Json;

namespace BaelorApi.Models.Api
{
	public class ResponseBase
	{
		[JsonProperty("result")]
		public object Result { get; set; }

		[JsonProperty("error")]
		public ErrorBase Error { get; set; } = null;

		[JsonProperty("success")]
		public bool Success { get; set; } = true;

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}