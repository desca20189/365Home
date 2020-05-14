using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using _365Home.Models;
using _365Home.DataAccess.Data.Repository;
using _365Home.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using _365Home.Ultility;
using _365Home.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _365Home.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private HomeViewModel HomeVM;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public HomeController(IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        //public IActionResult Index()
        //{
        //    HomeVM = new HomeViewModel()
        //    {
        //        CategoryList = _unitOfWork.Category.GetAll(),
        //        ServiceList = _unitOfWork.Service.GetAll(includeProperties: "Frequency"),
        //        LocationList = _unitOfWork.Location.GetAll(includeProperties: "Province,District,Ward,ApplicationUser,LocationType").Where(x => x.IsDeleted != true)
        //    };
        //    return View(HomeVM);
        //}

        public IActionResult Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
           

            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var locations = _unitOfWork.Location
                    .GetAll(includeProperties: "Province,District,Ward,ApplicationUser,LocationType,ImageList")
                    .Where(x => (x.IsDeleted != true) && (x.IsPublic == true) && (x.LocationStatus != SD.LocationStatusBooked));
            HomeVM = new HomeViewModel();
            if (!String.IsNullOrEmpty(searchString))
            {
                locations = locations.Where(x => x.Name.ToUpper().Contains(searchString.ToUpper())
                || x.Address.ToUpper().Contains(searchString.ToUpper())
                || x.Province.Name.ToUpper().Contains(searchString.ToUpper())
                || x.District.Name.ToUpper().Contains(searchString.ToUpper())
                || x.Ward.Name.ToUpper().Contains(searchString.ToUpper())
                );
            }
            locations = sortOrder switch
            {
                "name_desc" => locations.OrderByDescending(x => x.Name),
                "Price" => locations.OrderBy(x => x.Price),
                "price_desc" => locations.OrderByDescending(x => x.Price),
                _ => locations.OrderBy(x => x.Name),
            };
            int pageSize = 9;
            HomeVM.LocationList = locations;
            //var temp = await PaginatedList<Location>.CreateAsync(locations.AsQueryable().AsNoTracking(), pageNumber ?? 1, pageSize);
            HomeVM.paginatedLocationList = PaginatedList<Location>.Create(locations.AsQueryable(), pageNumber ?? 1, pageSize);
            
            return View(HomeVM);
        }

        public IActionResult Details(int id)
        {
            var serviceFromDb = _unitOfWork.Service.GetFirstOrDefault(includeProperties: "Category,Frequency", filter: c => c.Id == id);
            return View(serviceFromDb);
        }

        public IActionResult LocationDetails(string id)
        {
            if(_unitOfWork.Location.GetFirstOrDefault(x => x.Id == id) == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var locationFromDb = _unitOfWork.Location.GetFirstOrDefault(includeProperties: "Province,District,Ward,ApplicationUser,LocationType,ImageList", filter: x => x.Id == id);
            locationFromDb.RentTimeStartDate = DateTime.Now;
            locationFromDb.RentTimeEndDate = DateTime.Now.AddDays(1);
            if(TempData["RentTimeEndDateError"] != null)
            {
                ViewBag.RentTimeEndDateError = TempData["RentTimeEndDateError"].ToString();
            }
            //List<UserAddress> userAddressList = new List<UserAddress>();
            //IEnumerable<SelectListItem> addressesList = null;
            //if (_signInManager.IsSignedIn(User)){
            //    var user = User.Identity.Name;
            //    var userId = _userManager.GetUserId(User);
            //    userAddressList.AddRange(_unitOfWork.UserAddress.GetAll().Where(x => x.UserId == userId));
            //    addressesList = _unitOfWork.UserAddress.GetUserAddressListForDropDownBaseOnUser(userId);
            //}

            CartLocationViewModel model = new CartLocationViewModel
            {
                locationId = locationFromDb.Id,
                Location = locationFromDb,
                RentTimeStartDate = DateTime.Now,
                RentTimeEndDate = DateTime.Now.AddDays(1),
                LocationBooked = false
                
                //UserAddressList = userAddressList,
                //UserAddressesList = addressesList

            };

            if (_signInManager.IsSignedIn(User))
            {
                var user = User.Identity.Name;
                var userId = _userManager.GetUserId(User);
                ApplicationUser userObj = _unitOfWork.User.Get(userId);
                List<OrderHeader> orderHeaderList = _unitOfWork.OrderHeader.GetAll(filter: x => x.UserId == userId).ToList();
                List<OrderDetails> orderDetailList = new List<OrderDetails>();

                foreach(OrderHeader orderHeader in orderHeaderList)
                {
                    OrderDetails orderDetail = _unitOfWork.OrderDetails.FindBaseOnOrderHeaderId(orderHeader.Id);
                    orderDetailList.Add(orderDetail);
                }

                var orderDetailToCheck = orderDetailList.Find(x => (x.LocationId == locationFromDb.Id) && (x.Status != "Rejected") && (x.Status != "Cancelled"));
                if(orderDetailToCheck != null)
                {
                    model.LocationBooked = true;
                }
            }


            return View(model);
        }
       
        //Add Location To Cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(CartLocationViewModel cartItem)
        {
            ModelState.Remove("Location.Area");
            ModelState.Remove("Location.LocationTypeId");
            ModelState.Remove("Location.RentTimeEndDate");
            ModelState.Remove("Location.RentTimeStartDate");
            if (ModelState.IsValid)
            {
                if (cartItem.RentTimeEndDate < cartItem.RentTimeStartDate)
                {
                    //ViewBag.RentTimeEndDateError = "Rent Time End Date must be greater or equal to Rent Time Start Date";
                    TempData["RentTimeEndDateError"] = "Rent Time End Date must be greater or equal to Rent Time Start Date";
                    ModelState.AddModelError("RentTimeEndDate", "Rent Time End Date must be greater or equal to Rent Time Start Date");
                    return RedirectToAction("LocationDetails", new { id = cartItem.Location.Id });
                }
                TempData["RentTimeEndDateError"] = "";
                //List<string> sessionList = new List<string>();
                List<CartLocationViewModel> cartList = new List<CartLocationViewModel>();
                CartLocationViewModel cartModel = new CartLocationViewModel()
                {
                    locationId = cartItem.Location.Id,
                    RentTimeStartDate = cartItem.RentTimeStartDate,
                    RentTimeEndDate = cartItem.RentTimeEndDate
                    //AddressId = cartItem.AddressId
                };

                if (HttpContext.Session.GetObject<List<CartLocationViewModel>>(SD.SessionCart) == null)
                {
                    cartList.Add(cartModel);
                    HttpContext.Session.SetObject(SD.SessionCart, cartList);
                }
                else
                {
                    cartList = HttpContext.Session.GetObject<List<CartLocationViewModel>>(SD.SessionCart);
                    bool itemInCart = false;
                    foreach (CartLocationViewModel model in cartList)
                    {
                        if (model.locationId == cartItem.Location.Id)
                        {
                            itemInCart = true;
                        }
                    }
                    if (!itemInCart)
                    {
                        cartList.Add(cartModel);
                    }
                    HttpContext.Session.SetObject(SD.SessionCart, cartList);

                }
                //if (string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
                //{
                //    sessionList.Add(locationId);
                //    HttpContext.Session.SetObject(SD.SessionCart, sessionList);
                //}

                //else
                //{
                //    sessionList = HttpContext.Session.GetObject<List<string>>(SD.SessionCart);
                //    if (!sessionList.Contains(locationId))
                //    {
                //        sessionList.Add(locationId);
                //        HttpContext.Session.SetObject(SD.SessionCart, sessionList);
                //    }
                //}
                //var test = location.RentTimeFrom;
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult GetAddressInfoBaseOnAddressId(string addressId)
        {
            if(addressId != null)
            {
                return Json(new { data = _unitOfWork.UserAddress.Get(addressId) });
            }
            else
            {
                return Json(new { data = ""});
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
