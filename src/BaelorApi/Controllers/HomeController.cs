using Microsoft.AspNet.Mvc;

namespace BaelorApi.Controllers
{
	public class HomeController : Controller
	{
		// GET: /<controller>/
		public IActionResult Index()
		{
			return View();
		}
	}
}
