using System;
using System.Linq;
using Jose;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using SharpRaven.Core;

namespace Baelor.Attributes
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class RequireAuthenticationAttribute : Attribute, IActionFilter
	{
		private IRavenClient _ravenClient;

		public RequireAuthenticationAttribute(IRavenClient ravenClient)
		{
			_ravenClient = ravenClient;
		}

		public void OnActionExecuted(ActionExecutedContext context) { }

		public void OnActionExecuting(ActionExecutingContext context)
		{
			var request = context.HttpContext.Request;

			StringValues authHeader = default(StringValues);
			request.Headers.TryGetValue("Authorization", out authHeader);
			if (authHeader.Count != 1)
				throw new IndexOutOfRangeException("Invalid Auth Header");

			var privateKey = new byte[]
			{
				0xd8, 0x65, 0x06, 0xe7, 0xa2, 0xde, 0x15, 0xd5, 0xc7, 0xc9, 0x91, 0x23,
				0xf9, 0xad, 0xf1, 0x97, 0xca, 0x84, 0x9d, 0x4e, 0x04, 0xe3, 0xf8, 0xff,
				0x02, 0xcc, 0x17, 0x95, 0x6a, 0x31, 0x04, 0x59
			};

			var token = authHeader.First();
			var json = JWT.Decode(token, privateKey, JweAlgorithm.ECDH_ES_A256KW, JweEncryption.A128CBC_HS256);
		}
	}
}
