using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;


namespace InventoryManagementSystem.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<IdentityUser> userManager,
                                      SignInManager<IdentityUser> signInManager,
                                      IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid");

            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, true, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Credentials");

            }
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordModel forgetPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(forgetPassword.Email);
                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        TempData["error"] = "Could not reset the password";
                        return View();
                    }
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var EmailConfirmationUrl = Url.Action(
                                        "ResetPassword",
                                        "Account",
                                        new { code = code, email = forgetPassword.Email },
                    Request.Scheme
                                        );
                    await _emailSender.SendEmailAsync(
                        forgetPassword.Email,
                        "Reset Password",
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(EmailConfirmationUrl)}'>clicking here</a>.");
                    return RedirectToAction(nameof(ForgetPasswordConfirmation));
                }
                return View();
            }
            catch
            {
                throw;
            }
        }
        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }
        public IActionResult ResetPassword(string code = null, string email = null)
        {
            if (code == null || email == null)
            {
                TempData["error"] = "Problem occurred while resetting the password";
                return RedirectToAction("Login");
            }
            else
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.Code = code;
                model.Email = email;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    TempData["error"] = "Could not reset password";
                    return RedirectToAction(nameof(Login));
                }
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }
            catch
            {
                throw;
            }
        }
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }

}







