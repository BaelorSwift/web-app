using Microsoft.AspNet.Mvc;
using System.Web.Http;
using BaelorApi.Models.Repositories;
using BaelorApi.Models.ViewModels;
using System.Text.RegularExpressions;
using BaelorApi.Extentions;
using BaelorApi.Exceptions;
using BaelorApi.Models.Error.Enums;
using System.Net;
using BaelorApi.Cryptography;
using BaelorApi.Models.Database;
using BaelorApi.Helpers;
using BaelorApi.Models.Api;
using BaelorApi.Attributes;
using BaelorApi.Models.Miscellaneous;

namespace BaelorApi.Areas.Api.v0.Controllers
{
	[ExceptionHandler]
	[SetResponseHeaders]
	[Route("api/v0/[controller]")]
	public class SessionsController : ApiController
	{
		/// <summary>
		/// Repository for interacting with <see cref="User"/> data
		/// </summary>
		private readonly IUserRepository _userRepository;

		/// <summary>
		/// Creates a new instance of the Users Controller.
		/// </summary>
		/// <param name="UserRepository">The repository of <see cref="User"/> data.</param>
		public SessionsController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		/// <summary>
		///		[POST] api/v0/sessions
		/// Create a session
		/// </summary>
		/// <param name="value">A <see cref="CreateSessionViewModel"/> containing the data to get the user from.</param>
		[HttpPost]
		public IActionResult Post([FromBody] CreateSessionViewModel viewModel)
		{
			User user = null;
			if (ModelState.IsValid)
			{
				#region [ Validate User ]

				user = _userRepository.GetByUsername(viewModel.Identity);
				if (user == null)
					user = _userRepository.GetByEmailAddress(viewModel.Identity);
				if (user == null)
					ModelState.AddModelError("Identity", "invalid");
				else
				{
					if (!Pbkdf2.ValidateHash(viewModel.Password, new Pbkdf2Container
					{
						Hash = user.PasswordHash,
						Iterations = user.PasswordIterations,
						Salt = user.PasswordSalt
					}))
					{
						ModelState.AddModelError("Identity", "invalid");
					}
				}

				#endregion
			}
			
			#region [ Model Validation ]

			var errors = ModelState.GetErrors("viewModel");
			if (errors != null)
				throw new BaelorV0Exception(ErrorStatus.DataValidationFailed, HttpStatusCode.BadRequest, errors);

			#endregion
			
			return Content(HttpStatusCode.OK, new ResponseBase
			{
				Success = true,
				Error = null,
				Result = Models.Api.Response.Partials.User.Create(user)
			});
		}
	}
}
