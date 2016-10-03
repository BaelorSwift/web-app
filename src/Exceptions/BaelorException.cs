using System;

namespace Baelor.Exceptions
{
	public class BaelorException : Exception
	{
		public BaelorException(string message, Exception ex)
			: base(message, ex)
		{ }
	}
}
