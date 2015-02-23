using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BaelorApi.Helpers
{
	/// <summary>
	/// 
	/// </summary>
	public static class DopeTrace
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="lineNumber"></param>
		/// <param name="filePath"></param>
		/// <param name="memberName"></param>
		public static void WriteLine(object data, 
			[CallerLineNumber] int lineNumber = 0,
			[CallerFilePath] string filePath = "",
			[CallerMemberName] string memberName = "")
		{
			Trace.WriteLine(
				string.Format("[{0}:{1}] {2} - {3}", filePath, memberName, lineNumber, data));
		}
	}
}