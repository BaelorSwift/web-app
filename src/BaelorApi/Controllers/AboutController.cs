using Microsoft.AspNet.Mvc;

namespace BaelorApi.Controllers
{
	public class AboutController : Controller
	{
		// GET: /<controller>/
		public IActionResult Index()
		{
			return View();
		}
	}
}
