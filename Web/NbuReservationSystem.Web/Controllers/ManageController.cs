namespace NbuReservationSystem.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;

    using NbuReservationSystem.Web.ViewModels.Manage;

    [Authorize]
    public class ManageController : BaseController
    {
        private ApplicationSignInManager signInManager;
        private ApplicationUserManager userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,

            ChangePasswordSuccess,

            SetTwoFactorSuccess,

            SetPasswordSuccess,

            RemoveLoginSuccess,

            RemovePhoneSuccess,

            Error
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return this.signInManager ?? this.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }

            private set
            {
                this.signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager ?? this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                this.userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager => this.HttpContext.GetOwinContext().Authentication;

        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            this.ViewBag.StatusMessage = message == ManageMessageId.ChangePasswordSuccess
                                             ? "Your password has been changed."
                                             : message == ManageMessageId.SetPasswordSuccess
                                                   ? "Your password has been set."
                                                   : message == ManageMessageId.SetTwoFactorSuccess
                                                         ? "Your two-factor authentication provider has been set."
                                                         : message == ManageMessageId.Error
                                                               ? "An error has occurred."
                                                               : message == ManageMessageId.AddPhoneSuccess
                                                                     ? "Your phone number was added."
                                                                     : message == ManageMessageId.RemovePhoneSuccess
                                                                           ? "Your phone number was removed."
                                                                           : string.Empty;

            var userId = this.User.Identity.GetUserId();
            var model = new IndexViewModel
                            {
                                HasPassword = this.HasPassword(),
                                PhoneNumber = await this.UserManager.GetPhoneNumberAsync(userId),
                                TwoFactor = await this.UserManager.GetTwoFactorEnabledAsync(userId),
                                Logins = await this.UserManager.GetLoginsAsync(userId),
                                BrowserRemembered =
                                    await
                                    this.AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
                            };
            return this.View(model);
        }

        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return this.View();
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var result =
                await
                this.UserManager.ChangePasswordAsync(
                    this.User.Identity.GetUserId(),
                    model.OldPassword,
                    model.NewPassword);
            if (result.Succeeded)
            {
                var user = await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());
                if (user != null)
                {
                    await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                this.ViewBag.PasswordChangeSuccess = true;
                return this.View();
            }

            this.AddErrors(result);
            return this.View(model);
        }

        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return this.View();
        }

        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.UserManager.AddPasswordAsync(this.User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());
                    if (user != null)
                    {
                        await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }

                    return this.RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }

                this.AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.userManager != null)
            {
                this.userManager.Dispose();
                this.userManager = null;
            }

            base.Dispose(disposing);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error);
            }
        }

        private bool HasPassword()
        {
            var user = this.UserManager.FindById(this.User.Identity.GetUserId());
            return user?.PasswordHash != null;
        }
    }
}
