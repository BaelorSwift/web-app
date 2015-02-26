using Microsoft.AspNet.Mvc;

namespace BaelorApi.Controllers
{
	public class DocsController : Controller
	{
		// GET: /<controller>/
		public IActionResult Index()
		{
			return View();
		}
	}
}
