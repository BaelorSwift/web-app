using BaelorApi.Exceptions;
using BaelorApi.Models.Error.Enums;
using BaelorApi.Models.Miscellaneous;
using Microsoft.AspNet.Mvc;
using System.Net;

namespace BaelorApi.Attributes
{
	public class RequireAdminAuthenticationAttribute
		: ActionFilterAttribute
	{
		/// <summary>
		/// Checks if the request authentication allow admin actions.
		/// </summary>
		public RequireAdminAuthenticationAttribute() { }

		/// <summary>
		/// Check if the authenticated user has admin privileges.
		/// </summary>
		/// <param name="context">The action context.</param>
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			// Save user authentication
			var auth = context.HttpContext.GetFeature<AuthenticationStorage>();

			if (auth == null || !auth.IsAdmin)
				throw new BaelorV0Exception(ErrorStatus.InsufficientPrivileges, HttpStatusCode.Forbidden);

			base.OnActionExecuting(context);
		}
	}
}
