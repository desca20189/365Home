using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _365Home.DataAccess.Data.Repository;
using _365Home.Models;
using _365Home.Models.ViewModels;
using _365Home.Ultility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _365Home.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public OrderController(IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = SD.Admin)]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            if(id != 0)
            {
                OrderViewModel orderVM = new OrderViewModel
                {
                    OrderHeader = _unitOfWork.OrderHeader.Get(id),
                    OrderDetails = _unitOfWork.OrderDetails.GetAll(filter: o => o.OrderHeaderId == id, includeProperties: "Location")
                };
                return View(orderVM);
            }
            else
            {
                return View(nameof(OrderHistory));
            }
        }

        public IActionResult OrderHistoryDetails(int id)
        {
            OrderViewModel orderVM = new OrderViewModel
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(id),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(filter: o => o.OrderHeaderId == id, includeProperties: "Location"),
            };
            return View(orderVM);
        }

        public IActionResult Approve(int id)
        {
            var orderFromDb = _unitOfWork.OrderHeader.Get(id);
            if(orderFromDb == null)
            {
                return NotFound();
            }
            _unitOfWork.OrderHeader.ChangeOrderStatus(id, SD.StatusApproved);
            //Inside ChangeOrderStatus() already have .save() so no need for .save() here
            return View(nameof(Index));
        }
        public IActionResult Reject(int id)
        {
            var orderFromDb = _unitOfWork.OrderHeader.Get(id);
            if (orderFromDb == null)
            {
                return NotFound();
            }
            _unitOfWork.OrderHeader.ChangeOrderStatus(id, SD.StatusRejected);
            //Inside ChangeOrderStatus() already have .save() so no need for .save() here
            return View(nameof(Index));
        }

        public IActionResult ApproveOrderDetails(int id)
        {
            var orderDetailFromDb = _unitOfWork.OrderDetails.Get(id);
            if (orderDetailFromDb == null)
            {
                return Json(new { success = false, message = "Can't find order details." });
            }
            _unitOfWork.OrderDetails.ChangeOrderStatus(id, SD.StatusApproved);

            //UPDATE LOCATION STATUS
            var locationFromDb = _unitOfWork.Location.Get(orderDetailFromDb.LocationId);
            locationFromDb.LocationStatus = SD.LocationStatusBooked;
            _unitOfWork.Location.Update(locationFromDb);
            _unitOfWork.Save();
            //Inside ChangeOrderStatus() already have .save() so no need for .save() here

            return Json(new { success = true, message = "Order Approved." });
            //return View(nameof(LocationOrder));
        }
        public IActionResult RejectOrderDetails(int id)
        {
            var orderDetailFromDb = _unitOfWork.OrderDetails.Get(id);
            if (orderDetailFromDb == null)
            {
                return Json(new { success = false, message = "Can't find order details." });
            }
            _unitOfWork.OrderDetails.ChangeOrderStatus(id, SD.StatusRejected);

            //UPDATE LOCATION STATUS
            var locationFromDb = _unitOfWork.Location.Get(orderDetailFromDb.LocationId);
            locationFromDb.LocationStatus = SD.LocationStatusVacant;
            _unitOfWork.Location.Update(locationFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Order Rejected." });
            //Inside ChangeOrderStatus() already have .save() so no need for .save() here
            //return View(nameof(LocationOrder));
        }

        public IActionResult CancelOrderDetails(int id)
        {
            var orderDetailFromDb = _unitOfWork.OrderDetails.Get(id);
            if (orderDetailFromDb == null)
            {
                return View(nameof(OrderHistory));
            }
            _unitOfWork.OrderDetails.ChangeOrderStatus(id, SD.StatusCancelled);

            //UPDATE LOCATION STATUS
            var locationFromDb = _unitOfWork.Location.Get(orderDetailFromDb.LocationId);
            locationFromDb.LocationStatus = SD.LocationStatusVacant;
            _unitOfWork.Location.Update(locationFromDb);
            _unitOfWork.Save();

            return View(nameof(OrderHistory));
            //Inside ChangeOrderStatus() already have .save() so no need for .save() here
            //return View(nameof(LocationOrder));
        }

        public IActionResult LocationOrder() {
            return View();
        }

        public IActionResult OrderHistory()
        {
            return View();
        }

        #region API Calls
        [Authorize(Roles = SD.Admin)]
        public IActionResult GetAllOrders()
        {
            return Json(new { data = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser") });
        }
        [Authorize(Roles = SD.Admin)]
        public IActionResult GetAllPendingOrders()
        {
            return Json(new { data = _unitOfWork.OrderHeader.GetAll(filter: o => o.Status == SD.StatusSubmitted, includeProperties: "ApplicationUser") });
        }
        [Authorize(Roles = SD.Admin)]
        public IActionResult GetAllApprovedOrders()
        {
            return Json(new { data = _unitOfWork.OrderHeader.GetAll(filter: o => o.Status == SD.StatusApproved, includeProperties: "ApplicationUser") });
        }

        public IActionResult GetAllLocationsOrders()
        {
            var userId = "";
            if (_signInManager.IsSignedIn(User))
            {
                var user = User.Identity.Name;
                userId = _userManager.GetUserId(User);
            }
            return Json(new { data = _unitOfWork.OrderDetails.GetAll(includeProperties: "Location,ApplicationUser,OrderHeader").Where(x => x.UserId == userId) });
        }

        public IActionResult GetAllLocationsPendingOrders()
        {
            var userId = "";
            if (_signInManager.IsSignedIn(User))
            {
                var user = User.Identity.Name;
                userId = _userManager.GetUserId(User);
            }
            return Json(new { data = _unitOfWork.OrderDetails.GetAll(filter: o => o.Status == SD.StatusSubmitted, includeProperties: "Location").Where(x => x.UserId == userId) });
        }

        public IActionResult GetAllLocationsApprovedOrders()
        {
            var userId = "";
            if (_signInManager.IsSignedIn(User))
            {
                var user = User.Identity.Name;
                userId = _userManager.GetUserId(User);
            }
            return Json(new { data = _unitOfWork.OrderDetails.GetAll(filter: o => o.Status == SD.StatusApproved, includeProperties: "Location").Where(x => x.UserId == userId) });
        }

        public IActionResult GetAllOrdersHistory()
        {
            var userId = "";
            if (_signInManager.IsSignedIn(User)) {
                var user = User.Identity.Name;
                userId = _userManager.GetUserId(User);
                return Json(new { data = _unitOfWork.OrderHeader.GetAll().Where(x => x.UserId == userId) });
            }
            else
            {
                return Json(new EmptyResult()) ;
            }

        }

        public IActionResult GetPendingOrdersHistory()
        {
            var userId = "";
            if (_signInManager.IsSignedIn(User))
            {
                var user = User.Identity.Name;
                userId = _userManager.GetUserId(User);
            }
            return Json(new { data = _unitOfWork.OrderHeader.GetAll(filter: o => o.Status == SD.StatusSubmitted).Where(x => x.UserId == userId) });
        }

        public IActionResult GetApprovedOrdersHistory()
        {
            var userId = "";
            if (_signInManager.IsSignedIn(User))
            {
                var user = User.Identity.Name;
                userId = _userManager.GetUserId(User);
            }
            return Json(new { data = _unitOfWork.OrderHeader.GetAll(filter: o => o.Status == SD.StatusApproved).Where(x => x.UserId == userId) });
        }


        #endregion
    }
}