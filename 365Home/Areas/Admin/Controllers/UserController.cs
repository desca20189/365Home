using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using _365Home.DataAccess.Data.Repository;
using _365Home.Models;
using _365Home.Ultility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace _365Home.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Admin)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public UserController(IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            return View(_unitOfWork.User.GetAll(u=>u.Id!=claims.Value));
        }

        public IActionResult Lock(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            _unitOfWork.User.LockUser(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult UnLock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _unitOfWork.User.UnLockUser(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            return View(_unitOfWork.User.Get(id));
        }
        [HttpPost]
        public IActionResult Details(ApplicationUser user)
        {
            if(user == null)
            {
                return View(nameof(Index));
            }
            _unitOfWork.User.Update(user);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
 
        public async Task<IActionResult> ResendEmailVerification(string userId)
        {
            if (userId != null)
            {
                var user = _unitOfWork.User.Get(userId);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code = code },
                    protocol: Request.Scheme);

                //SEND EMAIL
                await _emailSender.SendEmailAsync(user.Email, "365Home - Confirm your email",
                    $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");
                return Json(new { success = true, message = "Email sent, please check your email." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to send verification email." });
            }
        }

        //public void ResendOTPPhoneNumber(string userId)
        //{
        //    if (userId != null)
        //    {
        //        var user = await _userManager.FindByIdAsync(userId);
        //        var result = await _userManager.AddToRoleAsync(user, SD.Admin);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
    }
}