using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace RememberASP.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private static bool _databaseChecked;
    private readonly ILogger logger;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<LettersController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        this.logger = logger;
    }

    public async Task<IActionResult> Register(string username, string email, string password, string? returnUrl = null)
    {
        if (username is null &&
            email is null &&
            password is null)
        {
            ViewData["ReturnUrl"] = Request.Headers.Referer.ToString();
            return View();
        }

        ApplicationUser user = new ApplicationUser
        {
            Email = email,
            UserName = username
        };

        var creationResult = await _userManager.CreateAsync(user, password);

        if (creationResult.Succeeded)
        {
            _signInManager.SignInAsync(user, isPersistent: false);

            logger.LogInformation($"Registered new user: {username}, {email}");
        }
        else
        {
            ViewData["Errors"] = creationResult.Errors;
            return View();
        }

        if (returnUrl is not null)
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
    {
        if (email is null &&
            password is null)
        {
            ViewData["ReturnUrl"] = returnUrl ?? Request.Headers.Referer.ToString();
            return View();
        }

        var signedUser = await _userManager.FindByEmailAsync(email);
        if (signedUser is not null)
        {
            var res = await _signInManager.PasswordSignInAsync(signedUser, password, false, false);

            if (!res.Succeeded)
            {
                ViewData["Error"] = "Invalid password.";
                return View();
            }
        }
        else
        {
            ViewData["Error"] = "No user with this email found.";
            return View();
        }

        logger.LogInformation($"User {signedUser.UserName} logged in.");

        if (returnUrl is not null)
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> Logout(string? returnUrl = null)
    {
        string name = User.Identity.Name;
        await _signInManager.SignOutAsync();

        logger.LogInformation($"User {name} logged out.");

        return RedirectToAction("Index", "Home");
    }
}
