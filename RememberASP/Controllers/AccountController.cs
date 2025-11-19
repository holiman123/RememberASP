using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace RememberASP.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private static bool _databaseChecked;
    private readonly ILogger _logger;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILoggerFactory loggerFactory)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = loggerFactory.CreateLogger<AccountController>();
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
            ViewData["ReturnUrl"] = Request.Headers.Referer.ToString();
            return View();
        }

        var signedUser = await _userManager.FindByEmailAsync(email);
        if (signedUser is not null)
        {
            var res = await _signInManager.PasswordSignInAsync(signedUser, password, false, false);
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
}
