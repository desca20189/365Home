using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _365Home.DataAccess.Data.Repository;
using _365Home.Extensions;
using _365Home.Models;
using _365Home.Models.ViewModels;
using _365Home.Ultility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _365Home.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        [BindProperty]
        public CartViewModel CartVM { get; set; }
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CartController(IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;

            CartVM = new CartViewModel()
            {
                OrderHeader = new Models.OrderHeader(),
                ServiceList = new List<Service>(),
                LocationList = new List<Location>(),
                CartLocationViewModel = new CartLocationViewModel()
            };
        }

        public IActionResult Index()
        {
            //if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null) 
            //{
            //    List<int> sessionList = new List<int>();
            //    sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            //    foreach(int serviceId in sessionList)
            //    {
            //        CartVM.ServiceList.Add(_unitOfWork.Service.GetFirstOrDefault(u => u.Id == serviceId, includeProperties:"Frequency,Category"));
            //    }
            //}
            //if (HttpContext.Session.GetObject<List<string>>(SD.SessionCart) != null)
            //{
            //    List<string> sessionList = new List<string>();
            //    sessionList = HttpContext.Session.GetObject<List<string>>(SD.SessionCart);
            //    foreach (string locationId in sessionList)
            //    {
            //        CartVM.LocationList.Add(_unitOfWork.Location.GetFirstOrDefault(x => x.Id == locationId,
            //            includeProperties: "Province,District,Ward,ApplicationUser,LocationType,ImageList"));
            //    }
            //}
            if (HttpContext.Session.GetObject<List<CartLocationViewModel>>(SD.SessionCart) != null)
            {
                List<CartLocationViewModel> sessionList = new List<CartLocationViewModel>();
                sessionList = HttpContext.Session.GetObject<List<CartLocationViewModel>>(SD.SessionCart);
                foreach(CartLocationViewModel model in sessionList)
                {
                    CartVM.LocationList.Add(_unitOfWork.Location.GetFirstOrDefault(x => x.Id == model.locationId,
                        includeProperties: "Province,District,Ward,ApplicationUser,LocationType,ImageList"));

                    //CartVM.CartLocationViewModelList.Add(model);
                }
            }

            return View(CartVM);
        }
        public IActionResult Summary()
        {
            if(HttpContext.Session.GetObject<List<CartLocationViewModel>>(SD.SessionCart) != null)
            {
                List<CartLocationViewModel> sessionList = new List<CartLocationViewModel>();
                sessionList = HttpContext.Session.GetObject<List<CartLocationViewModel>>(SD.SessionCart);
                foreach (CartLocationViewModel model in sessionList)
                {
                    CartVM.LocationList.Add(_unitOfWork.Location.GetFirstOrDefault(x => x.Id == model.locationId,
                        includeProperties: "Province,District,Ward,ApplicationUser,LocationType,ImageList"));

                }
                if (_signInManager.IsSignedIn(User))
                {
                    var user = User.Identity.Name;
                    var userId = _userManager.GetUserId(User);
                    ApplicationUser userObj = _unitOfWork.User.Get(userId);
                    CartVM.OrderHeader.Address = userObj.StreetAddress;
                    CartVM.OrderHeader.Name = userObj.Name;
                    CartVM.OrderHeader.Phone = userObj.PhoneNumber;
                    CartVM.OrderHeader.Email = userObj.Email;
                }
                if (TempData["BookingListValidationMessage"] != null)
                {
                    ViewBag.BookingListValidationMessage = TempData["BookingListValidationMessage"].ToString();
                }
            }
            return View(CartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            if (HttpContext.Session.GetObject<List<CartLocationViewModel>>(SD.SessionCart) != null)
            {
                //GET CURRENT BOOKING LIST
                List<CartLocationViewModel> sessionList = new List<CartLocationViewModel>();
                sessionList = HttpContext.Session.GetObject<List<CartLocationViewModel>>(SD.SessionCart);
                CartVM.LocationList = new List<Location>();
                foreach(CartLocationViewModel model in sessionList)
                {
                    CartVM.LocationList.Add(_unitOfWork.Location.Get(model.locationId));
                }
                foreach (CartLocationViewModel model in sessionList)
                {
                    var location = CartVM.LocationList.SingleOrDefault(x => x.Id == model.locationId);
                    location.RentTimeStartDate = model.RentTimeStartDate;
                    location.RentTimeEndDate = model.RentTimeEndDate;
                }             
            }
            if (!ModelState.IsValid)
            {
                return View(CartVM);
            }
            else
            {
                if (_signInManager.IsSignedIn(User))
                {
                    //GET USER
                    var user = User.Identity.Name;
                    var userId = _userManager.GetUserId(User);

                    //GET BOOKING LIST
                    List<OrderDetails> orderDetailsList = new List<OrderDetails>();
                    List<CartLocationViewModel> sessionList = new List<CartLocationViewModel>();
                    sessionList.ToList();
                    sessionList = HttpContext.Session.GetObject<List<CartLocationViewModel>>(SD.SessionCart);

                    //CHECK IF ANY PROPERTY IS BOOKED
                    bool itemExist = false;
                    foreach(var location in CartVM.LocationList)
                    {
                        if(PropertyIsInBookingList(location.Id, userId)){
                            itemExist = true;
                        }
                    }
                    if (itemExist)
                    {
                        TempData["BookingListValidationMessage"] = "Please check your booking list as there is a property belongs to a pending booking.";
                        return RedirectToAction("Summary");
                    }
                    else
                    {
                        CartVM.OrderHeader.UserId = userId;
                        CartVM.OrderHeader.OrderDate = DateTime.Now;
                        CartVM.OrderHeader.Status = SD.StatusSubmitted;
                        CartVM.OrderHeader.LocationCount = CartVM.LocationList.Count;
                        _unitOfWork.OrderHeader.Add(CartVM.OrderHeader);
                        _unitOfWork.Save();

                        foreach (var item in CartVM.LocationList)
                        {
                            OrderDetails orderDetails = new OrderDetails
                            {
                                LocationId = item.Id,
                                OrderHeaderId = CartVM.OrderHeader.Id,
                                Price = item.Price,
                                RentTimeStartDate = item.RentTimeStartDate,
                                RentTimeEndDate = item.RentTimeEndDate,
                                UserId = item.UserId,
                                Status = SD.StatusSubmitted

                            };
                            orderDetailsList.Add(orderDetails);
                            _unitOfWork.OrderDetails.Add(orderDetails);
                        }
                        _unitOfWork.Save();
                        //Clear Session
                        HttpContext.Session.SetObject(SD.SessionCart, new List<CartLocationViewModel>());
                        TempData["BookingListValidationMessage"] = "";
                        return RedirectToAction("OrderConfirmation", "Cart", new { id = CartVM.OrderHeader.Id });
                    }
                }
                else
                {
                    TempData["BookingListValidationMessage"] = "Please log in";
                    return View(CartVM);
                }
            }
        }

        public bool PropertyIsInBookingList(string locationId, string userId)
        {
            if(userId != null && locationId != null)
            {
                List<OrderHeader> orderHeaders = _unitOfWork.OrderHeader.GetAll(x => x.UserId == userId).ToList();

                List<OrderDetails> orderDetailList = new List<OrderDetails>();
                foreach(OrderHeader oh in orderHeaders)
                {
                    var orderDetail = _unitOfWork.OrderDetails.FindBaseOnOrderHeaderIdAndStatus(oh.Id, SD.StatusSubmitted);
                    if(orderDetail != null)
                    {
                        orderDetailList.Add(orderDetail);
                    }
                }

                var locationfromDB = _unitOfWork.Location.Get(locationId);
                var locationExist = orderDetailList.Find(x => x.LocationId == locationfromDB.Id);
                if (locationExist != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }

        //public IActionResult Remove(int serviceId)
        //{
        //    List<int> sessionList = new List<int>();
        //    sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
        //    sessionList.Remove(serviceId);
        //    HttpContext.Session.SetObject(SD.SessionCart, sessionList);

        //    return RedirectToAction(nameof(Index));
        //}

        public IActionResult Remove(string locationId)
        {
            List<CartLocationViewModel> sessionList = new List<CartLocationViewModel>();
            sessionList = HttpContext.Session.GetObject<List<CartLocationViewModel>>(SD.SessionCart);
            var itemToRemove = sessionList.SingleOrDefault(x => x.locationId == locationId);
            sessionList.Remove(itemToRemove);
            HttpContext.Session.SetObject(SD.SessionCart, sessionList);

            return RedirectToAction(nameof(Index));
        }
    }
}