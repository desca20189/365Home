using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using _365Home.Areas.Admin.Controllers;
using _365Home.Extensions;
using _365Home.Ultility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OtpNet;

namespace _365Home.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class VerifyPhoneModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AccountVerificationController _SMS; 
        public VerifyPhoneModel(UserManager<IdentityUser> userManager, AccountVerificationController SMS)
        {
            _userManager = userManager;
            _SMS = SMS;
        }

        public string PhoneNumber { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadPhoneNumber();
            return Page();
        }

        protected string GenerateOTP()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            characters += alphabets + small_alphabets + numbers;

            int length = 10;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }


        public async Task<IActionResult> OnPostAsync()
        {           
            await LoadPhoneNumber();

            try
            {
                string generatedOTP = GenerateOTP();
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("OTP")))
                {
                    HttpContext.Session.SetString("OTP", generatedOTP);
                }
                
                string result = _SMS.Send(PhoneNumber, HttpContext.Session.GetString("OTP"));

                if(result.Contains("100"))
                {
                    return RedirectToPage("ConfirmPhone");
                }

                ModelState.AddModelError("", $"There was an error sending the verification code: {result}");
            }
            catch (Exception)
            {
                ModelState.AddModelError("",
                    "There was an error sending the verification code, please check the phone number is correct and try again");
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
