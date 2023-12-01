using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Server.Auth.Data.DbContext;
using Server.Auth.Data.Entityes;
using Server.Auth.Services.Infrastructure;
using Server.Auth.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Auth.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountServices _accountServices;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUsers> _signInManager;

        public HomeController(IAccountServices accountServices, ApplicationDbContext context, SignInManager<ApplicationUsers> signInManager, UserManager<ApplicationUsers> userManager)
        {
            _accountServices = accountServices;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegisterViewModel model)
        {
            const string role = "User";

            ApplicationUsers users = new ApplicationUsers();

            users.Id = Guid.NewGuid();
            users.ApplicationUserId = Guid.NewGuid();
            users.Enabled = true;
            users.TwoFactorEnabled = false;
            users.PhoneNumber = "00000000000";
            users.Email = model.Email;
            users.NormalizedEmail = model.Email.ToUpper();
            users.UserName = model.FirstName;
            users.FirstName = model.Email;
            users.LastName = role;
            users.NormalizedUserName = model.FirstName.ToUpper();
            users.EmailConfirmed = true;
            users.PhoneNumberConfirmed = true;
            users.SecurityStamp = Guid.NewGuid().ToString("D");
            users.LockoutEnd = DateTime.Now;
            users.LockoutEnabled = false;
            users.AccessFailedCount = 0;


            if (!_context.Users.Any(x => x.UserName == model.FirstName))
            {
                
                var password = new PasswordHasher<ApplicationUsers>();
                var hashPassword = password.HashPassword(users, model.Password);
                users.PasswordHash = hashPassword;

                var tt = users;

                var result = await _userManager.CreateAsync(users, users.PasswordHash);
                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(users, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }
    }
}
