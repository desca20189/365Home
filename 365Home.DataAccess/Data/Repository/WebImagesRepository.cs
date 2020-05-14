using _365Home.DataAccess.Data.Repository.IRepository;
using _365Home.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _365Home.DataAccess.Data.Repository
{
    public class WebImagesRepository : Repository<WebImages>, IWebImagesRepository
    {
        private readonly ApplicationDbContext _db;
        public WebImagesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Delete(WebImages image)
        {
            var objFromDb = _db.WebImages.Find(image);
            if(objFromDb != null)
            {
                _db.WebImages.Remove(objFromDb);
            }
            _db.SaveChanges();
        }
    }
}
