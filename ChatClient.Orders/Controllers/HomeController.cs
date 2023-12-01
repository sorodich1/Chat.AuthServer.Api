using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatClient.Orders.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string x)
        {
            var accessToken = await HttpContext.AuthenticateAsync(x);

            var client = new HttpClient();

            return View();
        }
    }
}
