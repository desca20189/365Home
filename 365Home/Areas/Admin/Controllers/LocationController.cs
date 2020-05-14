using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using _365Home.DataAccess.Data;
using _365Home.DataAccess.Data.Repository;
using _365Home.Models;
using _365Home.Models.ViewModels;
using _365Home.Ultility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _365Home.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class LocationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;
        public int HCM_Code = 79;
        public int District_1_Code = 760;
        [BindProperty]
        public LocationViewModel LocationViewModel { get; set; }
        public LocationController(IUnitOfWork unitOfWork,
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(string id)
        {
            LocationViewModel = new LocationViewModel()
            {
                Location = new Location(),
                ProvinceList = _unitOfWork.Province.GetProvinceListForDropDown(),
                DistrictList = _unitOfWork.District.GetDistrictListForDropDownByProvinceId(HCM_Code),
                WardList = _unitOfWork.Ward.GetWardListForDropDownByDistrictId(District_1_Code),
                LocationTypeList = _unitOfWork.LocationType.GetLocationTypeListForDropDown(),
                LocationStatusList = _unitOfWork.Location.GetLocationStatusListForDropDown().Where(x => x.Value != null && (x.Value == SD.LocationStatusVacant || x.Value == SD.LocationStatusBooked))
            };
            if (id != null)
            {
                List<Location> locationFromDb = _unitOfWork.Location
                    .GetAll(includeProperties: "Province,District,Ward,ApplicationUser,LocationType,ImageList").Where(x => x.Id == id).ToList();
                LocationViewModel.Location = locationFromDb.FirstOrDefault();
                LocationViewModel.DistrictList = _unitOfWork.District.GetDistrictListForDropDownByProvinceId(locationFromDb.FirstOrDefault().ProvinceId);
                LocationViewModel.WardList = _unitOfWork.Ward.GetWardListForDropDownByDistrictId(locationFromDb.FirstOrDefault().DistrictId);
                LocationViewModel.LocationStatusList = _unitOfWork.Location.GetLocationStatusListForDropDown().Where(x => x.Value != null && (x.Value == SD.LocationStatusVacant || x.Value == SD.LocationStatusBooked) );
            }
            return View(LocationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            ModelState.Remove("Location.RentTimeEndDate");
            ModelState.Remove("Location.RentTimeStartDate");
            if (ModelState.IsValid)
            { 
                if (LocationViewModel.Location.Id == null)
                {
                    //Get Current User      
                    var user = User.Identity.Name;
                    var userId = _userManager.GetUserId(User);
                    LocationViewModel.Location.UserId = userId;

                    //Stamp Creation Date
                    LocationViewModel.Location.CreatedDateTime = DateTime.Now;

                    //New Location
                    _unitOfWork.Location.Add(LocationViewModel.Location);
                }
                else
                {
                    //Edit Location             
                    _unitOfWork.Location.Update(LocationViewModel.Location);
                }
                _unitOfWork.Save();
                //Insert Images
                if(LocationViewModel.Location.Id != null)
                {
                    var files = HttpContext.Request.Form.Files;
                    var fileCount = 0;
                    if (files.Count > 0)
                    {
                        foreach (IFormFile file in files)
                        {
                            WebImages image = new WebImages();
                            var fileName = file.FileName;
                            byte[] pic = null;
                            using (var fileStreamReader = files[fileCount].OpenReadStream())
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    fileStreamReader.CopyTo(memoryStream);
                                    pic = memoryStream.ToArray();
                                }
                            }
                            image.Picture = pic;
                            image.Name = fileName;
                            image.LocationId = LocationViewModel.Location.Id;
                            _db.WebImages.Add(image);
                            _db.SaveChanges();
                            fileCount++;
                        }
                    }
                }
                
                return RedirectToAction(nameof(Index));
            }
            else
            {
                LocationViewModel.ProvinceList = _unitOfWork.Province.GetProvinceListForDropDown();
                LocationViewModel.DistrictList = _unitOfWork.District.GetDistrictListForDropDownByProvinceId(HCM_Code);
                LocationViewModel.WardList = _unitOfWork.Ward.GetWardListForDropDownByDistrictId(District_1_Code);
                LocationViewModel.LocationTypeList = _unitOfWork.LocationType.GetLocationTypeListForDropDown();
                LocationViewModel.LocationStatusList = _unitOfWork.Location.GetLocationStatusListForDropDown().Where(x => x.Value != null && (x.Value == SD.LocationStatusBooked || x.Value == SD.LocationStatusVacant));   
                return View(LocationViewModel);
            }
        }

        #region Province District Ward
        [HttpGet]
        public JsonResult GetAllDistrictByProvinceId(int? id)
        {
            var district = _db.District.Where(x => x.ProvinceId == id).OrderBy(x => x.Name).ToList();
            //var district = _unitOfWork.District.GetDistrictListForDropDownByProvinceId(id);
            return Json(district);
            
        }

        public JsonResult GetAllWardByDistrictId(int? id)
        {
            var ward = _db.Ward.Where(x => x.DistrictId == id).OrderBy(x => x.Name).ToList();
            return Json(ward);
        }
        #endregion


        #region API CALLS
        [HttpDelete]
        public IActionResult RemoveLoctionPicture(int Id)
        {
            if(Id != 0)
            {
                var imageFromDb = _db.WebImages.Find(Id);
                _db.WebImages.Remove(imageFromDb);
                _db.SaveChanges();
                return Json(new { success = true, message = "Deleted successful" });
            }
            return Json(new { success = false, message = "Delete unsuccessful" });

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            //Get Current User      
            var user = User.Identity.Name;
            var userId = _userManager.GetUserId(User);
            JsonResult temp = null;

            if (User.IsInRole(SD.Admin)){
                temp = Json(new { data = _unitOfWork.Location.GetAll(includeProperties: "Province,District,Ward,ApplicationUser,LocationType").Where(x => x.IsDeleted != true) });
            }else if (User.IsInRole(SD.LocationOwner))
            {
                temp = Json(new { data = _unitOfWork.Location.GetAll(includeProperties: "Province,District,Ward,ApplicationUser,LocationType").Where(x => x.IsDeleted != true).Where(x => x.UserId == userId) });
            }

            //NORMAL CALL 
            return temp;

            //CALL USING STORE PROCEDURE
            //return Json(new { data = _unitOfWork.StoreProcedureCall.ReturnList<Location>(SD.usp_GetAllLocation, null) });
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var objFromDb = _unitOfWork.Location.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Delete unsuccessful" });
            }
            else
            {
                var submittedOrder = _unitOfWork.OrderDetails.FindBaseOnLocationIdAndStatusAndStillActive(objFromDb.Id, DateTime.Now, SD.StatusSubmitted);
                var approvedOrder = _unitOfWork.OrderDetails.FindBaseOnLocationIdAndStatusAndStillActive(objFromDb.Id, DateTime.Now, SD.StatusApproved);
                if(approvedOrder != null && submittedOrder != null)
                {
                    _unitOfWork.Location.Delete(objFromDb);
                    _unitOfWork.Save();
                    return Json(new { success = true, message = "Deleted successful" });

                }
                else
                {
                    return Json(new { success = false, message = "Location exist in submitted/approved booking." });
                }

            }
        }

        [HttpPost]
        [Authorize(Roles = SD.Admin)]
        public IActionResult LockLocation(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Incorrect locationID" });
            }
            var objFromDb = _unitOfWork.Location.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Can't find location" });
            }
            _unitOfWork.Location.LockLocation(objFromDb);
            return Json(new { success = true, message = "Location locked" });
        }

        [HttpPost]
        [Authorize(Roles = SD.Admin)]
        public IActionResult UnLockLocation(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Incorrect locationID" });
            }
            var objFromDb = _unitOfWork.Location.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Can't find location" });
            }
            _unitOfWork.Location.UnLockLocation(objFromDb);
            return Json(new { success = true, message = "Location status is now vacant" });
        }
        #endregion
    }
}