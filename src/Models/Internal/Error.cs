using System.Collections;

namespace Baelor.Models.Internal
{
	public class Error<T>
	{
		public Error() { }

		public Error(string code)
		{
			Code = code;
		}

		public Error(string code, T metadata)
		{
			Code = code;
			Metadata = metadata;
		}

		public string Code { get; set; }
		
		public T Metadata { get; set; }
	}

	public class Error
		: Error<object>
	{
		public Error() : base() { }

		public Error(string code)
			: base(code)
		{ }

		public Error(string code, IEnumerable metadata)
			: base(code, metadata)
		{ }
	}
}
