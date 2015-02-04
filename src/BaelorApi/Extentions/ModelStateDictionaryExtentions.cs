using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace BaelorApi.Extentions
{
	public static class ModelStateDictionaryExtentions
	{
		/// <summary>
		/// Gets all the errors in a <see cref="ModelStateDictionary"/>, and puts them in a dictionary with the value they fall under.
		/// </summary>
		/// <param name="modelState">The <see cref="ModelStateDictionary"/> in question.</param>
		/// <param name="model">The name of the view model.</param>
		public static Dictionary<string, List<string>> GetErrors(this ModelStateDictionary modelState, string model)
		{
			if (modelState.IsValid)
				return null;

			var errorDetails = new Dictionary<string, List<string>>();
			foreach (var value in modelState)
			{
				if (value.Value.ValidationState != ModelValidationState.Invalid)
					continue;

				var key = value.Key.Replace(string.Format("{0}.", model), "").ToLowerInvariant();
				if (errorDetails.ContainsKey(key))
					errorDetails[key].AddRange(value.Value.Errors.Select(e => e.ErrorMessage));
				else
					errorDetails.Add(key, value.Value.Errors.Select(e => e.ErrorMessage).ToList());

				errorDetails[key] = errorDetails[key].Distinct().ToList();
			}

			return errorDetails.Any() ? errorDetails : null;
		}
	}
}