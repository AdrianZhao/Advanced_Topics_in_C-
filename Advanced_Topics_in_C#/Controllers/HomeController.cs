using Lab02.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Lab02.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using Advanced_Topics_in_C_.Models;
using Advanced_Topics_in_C_.Data;

namespace Lab02.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Advanced_Topics_in_C_Context _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public HomeController(ILogger<HomeController> logger, Advanced_Topics_in_C_Context context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            string roleName = "Investigator";

            bool roleExists = await _roleManager.FindByNameAsync(roleName) != null;

            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetInvestigatorRole()
        {
            string roleName = "Investigator";

            User loggedIn = (User)_context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (loggedIn == null)
            {
                return Problem("User not found");
            }

            bool hasRoleAlready = await _userManager.IsInRoleAsync(loggedIn, roleName);

            if (!hasRoleAlready)
            {
                await _userManager.AddToRoleAsync(loggedIn, roleName);
                await _signInManager.RefreshSignInAsync(loggedIn);

            }

            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult ListAccounts()
        {
            // Get the current user
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Get the user's accounts
            var userAccounts = _context.Accounts.Where(a => a.UserId == currentUser.Id).ToList();

            return View(userAccounts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Deposit(int accountId)
        {
            Account account = _context.Accounts.FirstOrDefault(a => a.Id == accountId);
            return View(account);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Deposit(int accountId, int amount)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
            {
                return NotFound();
            }
            account.Deposit += amount;
            _context.SaveChanges();
            return RedirectToAction("ListAccounts");
        }


        [Authorize]
        [HttpGet]
        public IActionResult Withdraw(int accountId)
        {
            Account account = _context.Accounts.FirstOrDefault(a => a.Id == accountId);
            return View(account);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Withdraw(int accountId, int amount)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
            {
                return NotFound();
            }
            if (amount <= 0 || amount > account.Deposit)
            {
                ModelState.AddModelError("amount", "Invalid withdrawal amount.");
                return View(account);
            }
            account.Deposit -= amount;
            _context.SaveChanges();
            return RedirectToAction("ListAccounts");
        }

    }
}