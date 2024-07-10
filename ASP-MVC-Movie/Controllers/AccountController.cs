using ASP_MVC_Movie.Interfaces;
using ASP_MVC_Movie.Models;
using ASP_MVC_Movie.Role;
using ASP_MVC_Movie.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_MVC_Movie.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IUserManagementService _userManagementService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment environment, IUserManagementService userManagementService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
            _userManagementService = userManagementService;
        }


        [HttpGet]
        // This method returnes html file
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        // This method if for register
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.AvatarImage != null && model.AvatarImage.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "avatars");
                        Directory.CreateDirectory(uploadsFolder);

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.AvatarImage.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.AvatarImage.CopyToAsync(fileStream);
                        }

                        user.Avatar = uniqueFileName;
                        await _userManager.UpdateAsync(user);
                    }

                    await _userManager.AddToRoleAsync(user, ApplicationRoles.User);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Movie");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        // This method returns html file
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        // This method is for login
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Wrong password");
                    return View(model);
                }

                return RedirectToAction("Index", "Movie");
            }
            return View(model);
        }

        // This method id for logout

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Movie");
        }


        [HttpGet]
        // This method shows all user
        public async Task<IActionResult> ShowAllUsers()
        {
            var users = await _userManagementService.GetAllUsers();
            return View(users);
        }


        // This method removes user 
        public async Task<IActionResult> RemoveUser(string userId)
        {
            var success = await _userManagementService.RemoveUser(userId);
            if (success)
            {
                return RedirectToAction("ShowAllUsers");
            }
            return View("Error");
        }



        [HttpPost]
        //This method edits user info
        public async Task<IActionResult> UserInfoEdit(EditUserVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _userManagementService.EditUserInfo(userId, model);
            if (!success)
            {
                return View("Error");
            }
            return RedirectToAction("Index", "Movie");
        }



    }
}
