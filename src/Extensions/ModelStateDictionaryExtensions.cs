using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Baelor.Extensions
{
	public static class ModelStateDictionaryExtensions
	{
		public static IEnumerable Errors(this ModelStateDictionary modelState)
		{
			if (modelState.IsValid)
				return null;
			
			return modelState.ToDictionary(kvp => kvp.Key,
				kvp => kvp.Value.Errors
								.Select(e => e.ErrorMessage).ToArray())
								.Where(m => m.Value.Count() > 0);
		}
	}
}
