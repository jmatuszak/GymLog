using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GymLog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null) return RedirectToAction("Login", "Account");

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                if (role == "user")
                {
                    RedirectToAction("Index", "Home");
                }
                else if (role == "admin")
                {
                    var accountsVM = await _userManager.Users
                        .Select(user => new AccountVM
                        {
                            Id = user.Id,
                            Username = user.UserName,
                            Email = user.Email,
                        }).ToListAsync();
                    var users = await _userManager.Users.ToListAsync();
                    foreach(var u in users)
                    {
                        var accountVM = accountsVM.FirstOrDefault(a => a.Id == u.Id);
                        if (accountVM != null)
                        {
                            accountVM.Roles = (List<string>)await _userManager.GetRolesAsync(u);
                        }
                    }
                    return View(accountsVM);
                }
            }
            return RedirectToAction("Login", "Account");
        }
        public IActionResult Login()
        {
            var response = new LoginVM();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user,loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                TempData["Error"] = "Wrong credentials. Please try again.";
                return View(loginVM);
            }
            TempData["Error"] = "Wrong credentials. Please try again.";
            return View(loginVM);
        }

        public IActionResult Register()
        {
            var response = new RegisterVM();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email is already in use";
                return View(registerVM);
            }

            var newUser = new AppUser()
            {
                Email = registerVM.EmailAddress,
                UserName = registerVM.Username
            };
            var newUserRespnse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserRespnse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            return RedirectToAction("Index");
        }
        public IActionResult CreateAdmin()
        {
            var response = new RegisterVM();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email is already in use";
                return View(registerVM);
            }

            var newUser = new AppUser()
            {
                Email = registerVM.EmailAddress,
                UserName = registerVM.Username
            };
            var newUserRespnse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserRespnse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            //Delete User Workouts
            var workouts = await _context.Workouts.Include(x => x.WorkoutSegments)
                .ThenInclude(segment => segment.Sets)
                .Where(x => x.AppUserId == user.Id).ToListAsync();
            if (workouts != null)
            {
                _context.Workouts.RemoveRange(workouts);
                _context.SaveChanges();
            }
            //Delete User Templates
            var templates = await _context.Workouts.Include(x => x.WorkoutSegments)
                .ThenInclude(segment => segment.Sets)
                .Where(x => x.AppUserId == user.Id).ToListAsync();
            if (templates != null)
            {
                _context.Workouts.RemoveRange(templates);
                _context.SaveChanges();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
