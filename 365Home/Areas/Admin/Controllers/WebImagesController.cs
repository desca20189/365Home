using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using _365Home.DataAccess.Data;
using _365Home.DataAccess.Data.Repository;
using _365Home.Models;
using _365Home.Ultility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _365Home.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class WebImageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        
        public WebImageController(IUnitOfWork unitOfWork,
            ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            WebImages imageObj = new WebImages();
            if (id == null)
            {
                
            }
            else
            {
                imageObj = _db.WebImages.SingleOrDefault(m => m.Id == id);
                if (imageObj == null)
                {
                    return NotFound();
                }
            }
            return View(imageObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(int id, WebImages imageObj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                var fileCount = 0;
                if(files.Count > 0)
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
                        var test = imageObj.Id;
                        //imageObj.Id = 0;
                        _db.WebImages.Add(image);
                        //_db.WebImages.AddRange(imageObj);
                        //_db.WebImages.AddRangeAsync(imageObj);

                        //CREATE NEW
                        //if (imageObj.Id == 0)
                        //{
                        //    _db.WebImages.Add(imageObj);
                        //}
                        ////EDIT
                        //else
                        //{
                        //    var imageFromDb = _db.WebImages.Where(c => c.Id == id).FirstOrDefault();
                        //    //imageFromDb.Name = imageObj.Name;
                        //    if (files.Count > 0)
                        //    {
                        //        imageFromDb.Picture = imageObj.Picture;
                        //    }
                        //}

                        _db.SaveChanges();
                        fileCount++;
                    }
                    

                }
                return RedirectToAction(nameof(Index));
            }
            return View(imageObj);
        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            //NORMAL CALL USING UNIT OF WORK
            //return Json(new { data = _unitOfWork.WebImages.GetAll(includeProperties: "Location") });

            //CALL USING STORE PROCEDURE
            //return Json(new { data = _unitOfWork.StoreProcedureCall.ReturnList<Category>(SD.usp_GetAllCategory, null) });

            //CALL DIRECTLY FROM DB
     
            return Json(new { data = _db.WebImages.ToList() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _db.WebImages.Find(id);
            if(objFromDb == null)
            {
                return Json(new { success = false, message = "Delete unsuccessful" });
            }
            _db.WebImages.Remove(objFromDb);
            _db.SaveChanges();
            return Json(new { success = true, message = "Deleted successful" });
        }
        #endregion
    }
}