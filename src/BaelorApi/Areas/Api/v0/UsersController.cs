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
	public class UsersController : ApiController
	{
		/// <summary>
		/// Repository for interacting with <see cref="User"/> data
		/// </summary>
		private readonly IUserRepository _userRepository;

		/// <summary>
		/// Creates a new instance of the Users Controller.
		/// </summary>
		/// <param name="UserRepository">The repository of <see cref="User"/> data.</param>
		public UsersController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[RequireAuthentication]
		public IActionResult Get()
		{
			var authentication = Context.GetFeature<AuthenticationStorage>();
			var user = _userRepository.GetById(authentication.UserId);

			return Content(HttpStatusCode.OK, new ResponseBase
			{
				Success = true,
				Error = null,
				Result = Models.Api.Response.Partials.User.Create(user)
			});
		}
		
		/// <summary>
		///		[POST] api/v0/user/
		/// Creates a user from the post data.
		/// </summary>
		/// <param name="value">A <see cref="CreateUserViewModel"/> containing the data to create the <see cref="User"/> from.</param>
		[HttpPost]
		public IActionResult Post([FromBody] CreateUserViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				#region [ Validate Password Strength ]

				var complexity = 0;
				if (Regex.IsMatch(viewModel.Password ?? "", @"\d+"))
					complexity++;
				if (Regex.IsMatch(viewModel.Password ?? "", @"[a-z]+"))
					complexity++;
				if (Regex.IsMatch(viewModel.Password ?? "", @"[A-Z]+"))
					complexity++;
				if (Regex.IsMatch(viewModel.Password ?? "", @"[^a-zA-Z\d]+"))
					complexity++;

				if (complexity < 2)
					ModelState.AddModelError("Password", "complexity");

				#endregion

				#region [ Validate Password Confirm ]

				if (string.IsNullOrWhiteSpace(viewModel.Password) ||
					string.IsNullOrWhiteSpace(viewModel.Password) ||
					viewModel.Password != viewModel.PasswordConfirm)
					ModelState.AddModelError("Password", "confirmation");

				#endregion

				#region [ Validate Unique User ]

				if (_userRepository.GetByEmailAddress(viewModel.EmailAddress) != null ||
					_userRepository.GetByUsername(viewModel.Username) != null)
					ModelState.AddModelError("Identity", "obtained");

				#endregion
			}
			
			#region [ Model Validation ]

			var errors = ModelState.GetErrors("viewModel");
			if (errors != null)
				throw new BaelorV0Exception(ErrorStatus.DataValidationFailed, HttpStatusCode.BadRequest, errors);

			#endregion
			
			#region [ Password Hashing ]

			var hash = Pbkdf2.ComputeHash(viewModel.Password);

			#endregion
			
			#region [ Create User ]

			var user = new User
			{
				PasswordHash = hash.Hash,
				PasswordSalt = hash.Salt,
				PasswordIterations = hash.Iterations,
				Username = viewModel.Username,
				EmailAddress = viewModel.EmailAddress,
				ApiKey = RandomGeneratorHelper.GenerateRandomBase64String(Models.Database.User.ApiKeyLength),
				IsAdmin = false,
				IsRevoked = false
			};
			user = _userRepository.Add(user);

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
