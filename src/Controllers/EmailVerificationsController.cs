using System.Threading.Tasks;
using Baelor.ViewModels.EmailVerifications;
using Microsoft.AspNetCore.Mvc;
using Baelor.Database.Repositories.Interfaces;
using SharpRaven.Core;
using Baelor.Extensions;
using Baelor.Models.Internal;
using System;
using Baelor.Database.Models;
using Microsoft.AspNetCore.Hosting;
using SendGrid;

namespace Baelor.Controllers
{
	[Route("email_verifications")]
	public class EmailVerificationsController : Controller
	{
		private IUserRepository _userRepository;
		private IEmailVerificationRepository _emailVerificationRepository;
		private SendGridClient _sendGridClient;
		private IRavenClient _ravenClient;

		public EmailVerificationsController(IUserRepository userRepository,
			IEmailVerificationRepository emailVerificationRepository,
			SendGridClient sendGridClient, IRavenClient ravenClient)
		{
			_userRepository = userRepository;
			_emailVerificationRepository = emailVerificationRepository;
			_sendGridClient = sendGridClient;
			_ravenClient = ravenClient;
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody] CreateEmailVerificationViewModel model)
		{
			if (!ModelState.IsValid)
				return Json(new Error("validation_failed", ModelState.Errors()));

			// Check email is valid, and is not verified already
			var user = await _userRepository.GetByEmailAddress(model.EmailAddress);
			if (user == null)
				return Json(new Error("user_not_found"));
			if (user.IsVerified)
				return Json(new Error("user_already_verified"));

			// Revoke all old Email Verification Codes
			await _emailVerificationRepository.RevokeAllCodes(user.Id);

			// Create Email Verification Code
			var emailVerification = new EmailVerification(user.Id);
			await _emailVerificationRepository.Add(emailVerification);

			// Send Code to email
			try
			{
				var url = $"/users/verify?code={emailVerification.Code}";
				if (Startup.HostingEnvironment.IsDevelopment())
					url = "http://localhost:3000" + url;
				else
					url = "https://baelor.io" + url;

				await _sendGridClient.MailClient.SendAsync(
					to: user.EmailAddress,
					toName: user.Name,
					subject: "Verify your Baelor.io Account",
					htmlBody: $"To verify your account, please click the following link: <a href=\"{url}\">{url}</a>",
					textBody: $"To verify your account, please following this link: {url}",
					@from: "support@baelor.io",
					fromName: "BaelorFromName.io"
				);
			}
			catch (Exception ex)
			{
				await _ravenClient.CaptureAsync("email_transport_failed", ex);
			}

			return Ok();
		}

		[HttpPost("verify")]
		public async Task<IActionResult> VerifyAsync([FromBody] VerifyUserViewModel model)
		{
			if (!ModelState.IsValid)
				return Json(new Error("validation_failed", ModelState.Errors()));

			// Get Verification
			var emailVerification = await _emailVerificationRepository.GetByCode(model.Code);
			if (emailVerification == null)
				return Json(new Error("invalid_code"));

			// Check Is hasn't been used and hasn't expired
			if (emailVerification.Used)
				return Json(new Error("code_used"));
			if (emailVerification.ExpiresAt < DateTime.UtcNow)
				return Json(new Error("code_expired"));

			// Flag user as verified
			await _userRepository.SetUserVerifiedStatus(emailVerification.UserId, true);

			// Return User Model
			return Json(await _userRepository.GetById(emailVerification.UserId));
		}
	}
}
