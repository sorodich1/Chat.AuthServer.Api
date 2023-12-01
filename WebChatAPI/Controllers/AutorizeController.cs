using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebChatAPI.Controllers
{
    public class AutorizeController : Controller
    {
       [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
