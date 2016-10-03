using System;
using System.Threading.Tasks;
using Baelor.Exceptions;
using SharpRaven.Core;
using SharpRaven.Core.Data;

namespace Baelor.Extensions
{
	public static class IRavenClientExtensions
	{
		public static async Task CaptureAsync(this IRavenClient client, string message, Exception ex)
		{
			var helmException = new BaelorException(message, ex);
			await client.CaptureAsync(new SentryEvent(ex));
		}

		public static async Task CaptureAsync(this IRavenClient client, ErrorLevel level,
			string message)
		{
			await client.CaptureAsync(new SentryEvent(new SentryMessage(message))
			{
				Level = level
			});
		}

		public static async Task CaptureAsync(this IRavenClient client, ErrorLevel level,
			string format, params string[] messages)
		{
			await client.CaptureAsync(new SentryEvent(new SentryMessage(format, messages))
			{
				Level = level
			});
		}
	}
}
