using System.Security.Claims;
using System.Threading.Tasks;
using Infrastructure.Identity;
using Infrastructure.Mailing.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.MVC.Models.Identity;

namespace WebUI.MVC.Controllers.Identity
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Register()
        {
            return View("~/Views/Identity/Register.cshtml", new UserRegistrationModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationModel userModel)
        {
            if (!ModelState.IsValid) {
                return View("~/Views/Identity/Register.cshtml", userModel);
            }

            var user = new ApplicationUser() { Email = userModel.Email, UserName = userModel.Email };
            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (result.Succeeded) return RedirectToAction(nameof(Login));

            foreach (var error in result.Errors) {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("~/Views/Identity/Register.cshtml", userModel);
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            return View("~/Views/Identity/Login.cshtml", new UserLoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel userModel, string returnUrl = null)
        {
            if (!ModelState.IsValid) {
                return View("~/Views/Identity/Login.cshtml", userModel);
            }
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            if (user != null &&
                await _userManager.CheckPasswordAsync(user, userModel.Password)) {

                await _signInManager.SignInAsync(user, isPersistent: false);

                return Redirect(returnUrl ?? $"~/Home/{nameof(HomeController.Index)}");
            }

            ModelState.AddModelError("", "Invalid UserName or Password");
            return View("~/Views/Identity/Login.cshtml", new UserLoginModel());
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet, AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View("~/Views/Identity/ForgotPassword.cshtml");
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            if (ModelState.IsValid) {
                var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
                if (user == null) {
                    return View("~/Views/Identity/ForgotPasswordConfirmation.cshtml");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { UserId = user.Id, token = token }, "Https");

                _emailService.Send(forgotPassword.Email, "Reset Password",
                $"To reset password kindly follow the: <a href='{callbackUrl}'>link</a>");
                return View("~/Views/Identity/ForgotPasswordConfirmation.cshtml");
            }
            ModelState.AddModelError("", "Invalid Email");

            return View("~/Views/Identity/ForgotPassword.cshtml");
        }

        [HttpGet, AllowAnonymous]
        public IActionResult ResetPassword(string token = null)
        {
            return token == null ? View("Error") : View("~/Views/Identity/ResetPassword.cshtml");
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordModel)
        {
            if (!ModelState.IsValid) {
                return View("~/Views/Identity/ResetPassword.cshtml", resetPasswordModel);
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null) {
                return View("~/Views/Identity/ResetPasswordConfirmation.cshtml");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (result.Succeeded) {
                return View("~/Views/Identity/ResetPasswordConfirmation.cshtml");
            }

            foreach (var error in result.Errors) {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("~/Views/Identity/ResetPassword.cshtml", resetPasswordModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) {
                return RedirectToAction(nameof(Login));
            }

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user == null) {
                var email = result.Principal.FindFirstValue("email")
                            ?? result.Principal.FindFirstValue(ClaimTypes.Email)
                            ?? result.Principal.FindFirstValue(ClaimTypes.Upn);
                
                if (email != null) {
                    user = await _userManager.FindByEmailAsync(email);

                    if (user == null) {
                        user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
                        await _userManager.CreateAsync(user);
                    }

                    await _userManager.AddLoginAsync(user, new UserLoginInfo(info.LoginProvider, info.ProviderKey, info.LoginProvider));
                }
            }

            if (user == null) {
                return View("Error");
            }

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            await _signInManager.SignInAsync(user, false);

            return Redirect(returnUrl ?? $"~/Home/{nameof(HomeController.Index)}");
        }
    }
}