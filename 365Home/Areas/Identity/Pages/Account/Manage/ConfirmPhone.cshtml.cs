using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _365Home.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ConfirmPhoneModel : PageModel
    {
        //private readonly TwilioVerifySettings _settings;
        private readonly UserManager<IdentityUser> _userManager;

        public ConfirmPhoneModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public string PhoneNumber { get; set; }

        [BindProperty, Required, Display(Name = "Code")]
        public string VerificationCode { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            await LoadPhoneNumber();
            return Page();
        }

        public bool CheckOTP(string verificationCode)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("OTP")))
            {
                var temp = HttpContext.Session.GetString("OTP");
                if (verificationCode.Equals(HttpContext.Session.GetString("OTP"))){
                    return true;
                }
            }
            return false;
        }


        public async Task<IActionResult> OnPostAsync()
        {
            await LoadPhoneNumber();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (CheckOTP(VerificationCode))
                {
                    var identityUser = await _userManager.GetUserAsync(User);
                    identityUser.PhoneNumberConfirmed = true;
                    var updateResult = await _userManager.UpdateAsync(identityUser);

                    if (updateResult.Succeeded)
                    {
                        return RedirectToPage("ConfirmPhoneSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("", "There was an error confirming the verification code, please try again");
                    }
                }
                else
                {
                    //ModelState.AddModelError("", $"There was an error confirming the verification code: {verification}");
                    ModelState.AddModelError("", $"There was an error confirming the verification code: Error");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("",
                    "There was an error confirming the code, please check the verification code is correct and try again");
            }

            return Page();
        }

        private async Task LoadPhoneNumber()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new Exception($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            PhoneNumber = user.PhoneNumber;
        }
    }
}
