using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace Baelor.Attributes
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class RequireInternalAuthenticationAttribute : Attribute, IActionFilter
	{
		public RequireInternalAuthenticationAttribute()
		{
			
		}

		public void OnActionExecuted(ActionExecutedContext context) { }

		public void OnActionExecuting(ActionExecutingContext context)
		{
			var request = context.HttpContext.Request;

			StringValues authHeader = default(StringValues);
			request.Headers.TryGetValue("Authorization", out authHeader);
		}
	}
}
