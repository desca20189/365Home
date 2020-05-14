using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _365Home.DataAccess.Data.Repository;
using _365Home.Extensions;
using _365Home.Models;
using _365Home.Models.ViewModels;
using _365Home.Ultility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _365Home.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    public class UserAddressController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        [BindProperty]
        public UserAddress userAddress { get; set; }

        private readonly UserManager<IdentityUser> _userManager;

        public UserAddressController(IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        //GET Upsert
        public IActionResult Upsert(string id)
        {
            UserAddress userAddress = new UserAddress();
            if (id == null)
            {
                return View(userAddress);
            }
            userAddress = _unitOfWork.UserAddress.Get(id);
            if (userAddress == null)
            {
                return NotFound();
            }
            return View(userAddress);
        }

        //POST Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(UserAddress userAddress)
        {
            if (ModelState.IsValid)
            {
                var user = User.Identity.Name;
                var userId = _userManager.GetUserId(User);
                if (userAddress.Id == null)
                {
                    userAddress.UserId = userId;
                    _unitOfWork.UserAddress.Add(userAddress);
                    _unitOfWork.Save();
                    if (userAddress.IsDefaultAddress)
                    {
                        List<UserAddress> listAddress = _unitOfWork.UserAddress.GetAll().ToList();
                        listAddress.Remove(userAddress);
                        foreach (UserAddress address in listAddress)
                        {
                            _unitOfWork.UserAddress.SetIsDefaultAddressToFalse(address);
                        }
                    }
                }
                else
                {
                    userAddress.UserId = userId;
                    _unitOfWork.UserAddress.Update(userAddress);
                    if (userAddress.IsDefaultAddress)
                    {
                        List<UserAddress> listAddress = _unitOfWork.UserAddress.GetAll().ToList();
                        foreach (UserAddress address in listAddress)
                        {
                            if (address.Id != userAddress.Id)
                            {
                                _unitOfWork.UserAddress.SetIsDefaultAddressToFalse(address);
                            }
                        }
                    }
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(userAddress);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var user = User.Identity.Name;
            var userId = _userManager.GetUserId(User);
            return Json(new { data = _unitOfWork.UserAddress.GetAll().Where(x => x.UserId == userId) });
        }
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var objFromDb = _unitOfWork.UserAddress.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }
            _unitOfWork.UserAddress.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete success." });
        }
        #endregion

    }
}