using Microsoft.AspNet.Mvc;

namespace BaelorApi.Attributes
{
	public class SetResponseHeadersAttribute
		: ActionFilterAttribute
	{
		/// <summary>
		/// Sets the response headers.
		/// </summary>
		public SetResponseHeadersAttribute() { }

		/// <summary>
		/// Set the response headers.
		/// </summary>
		/// <param name="context">The action context.</param>
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			context.HttpContext.Response.Headers
				.Add("X-BaelorApi-Version", new[] { "v0" });
			context.HttpContext.Response.Headers
				.Add("X-Bad-Mistakes", new[] { "add me on tinder xox" });
			context.HttpContext.Response.Headers
				.Add("X-Purple-Swag", new[] { "i still sip" });
			base.OnActionExecuting(context);
		}
	}
}
