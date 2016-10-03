using System.Threading.Tasks;
using Baelor.Extensions;
using Baelor.ViewModels.Clients;
using Microsoft.AspNetCore.Mvc;
using Baelor.Database.Repositories.Interfaces;
using SharpRaven.Core;
using Baelor.Models.Internal;
using Baelor.Database.Models;
using MongoDB.Bson;
using System.Linq;

namespace Baelor.Controllers
{
	[Route("[controller]")]
	public class ClientsController : Controller
	{
		private IUserRepository _userRepository;
		private IClientRepository _clientRepository;
		private IRavenClient _ravenClient;

		public ClientsController(IUserRepository userRepository,
			IClientRepository clientRepository,
			IRavenClient ravenClient)
		{
			_userRepository = userRepository;
			_clientRepository = clientRepository;
			_ravenClient = ravenClient;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			return Json(await _clientRepository.All());
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody] CreateClientViewModel model)
		{
			if (!ModelState.IsValid)
				return Json(new Error("validation_failed", ModelState.Errors()));

			// Create Client
			var client = new Client(model)
			{
				UserId = ObjectId.Parse("57f2b3881f1e03fc26b57b98")
			};

			// Get all user's clients
			var clients = await _clientRepository.GetClientsByUser(ObjectId.Parse("57f2b3881f1e03fc26b57b98"));

			// Check client is unique
			if (clients.Any(c => c.Slug == client.Slug))
				return Json(new Error("client_exists"));

			// Create ApiKey
			client.ApiKeys.Add(new ApiKey("default"));

			// Insert into db
			await _clientRepository.Add(client);

			// Return Client
			return Json(client);
		}
	}
}
