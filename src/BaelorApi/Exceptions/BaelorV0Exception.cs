using BaelorApi.Models.Error.Enums;
using System;
using System.Net;

namespace BaelorApi.Exceptions
{
	public class BaelorV0Exception : Exception
	{
		/// <summary>
		/// Creates a new Custom Oedipus exception for the V0 endpoint.
		/// </summary>
		/// <param name="errorStatus">The <see cref="Models.Api.Enums.ErrorStatus"/> of the exception.</param>
		/// <param name="httpStatusCode">The <see cref="System.Net.HttpStatusCode"/> of the exception.</param>
		public BaelorV0Exception(ErrorStatus errorStatus, HttpStatusCode httpStatusCode)
		{
			ErrorStatus = errorStatus;
			HttpStatusCode = httpStatusCode;
		}

		/// <summary>
		/// Creates a new Custom Oedipus exception for the V0 endpoint.
		/// </summary>
		/// <param name="errorStatus">The <see cref="Models.Api.Enums.ErrorStatus"/> of the exception.</param>
		/// <param name="httpStatusCode">The <see cref="System.Net.HttpStatusCode"/> of the exception.</param>
		/// <param name="errorDetails">An <see cref="object"/> containg the error details.</param>
		public BaelorV0Exception(ErrorStatus errorStatus, HttpStatusCode httpStatusCode, object errorDetails)
		{
			ErrorStatus = errorStatus;
			HttpStatusCode = httpStatusCode;
			ErrorDetails = errorDetails;
		}

		/// <summary>
		/// The <see cref="Models.Api.Enums.ErrorStatus"/> of the exception.
		/// </summary>
		public ErrorStatus ErrorStatus { get; set; }

		/// <summary>
		/// The <see cref="System.Net.HttpStatusCode"/> of the exception.
		/// </summary>
		public HttpStatusCode HttpStatusCode { get; set; }

		/// <summary>
		/// An <see cref="object"/> containg the error details.
		/// </summary>
		public object ErrorDetails { get; set; } = null;
	}
}