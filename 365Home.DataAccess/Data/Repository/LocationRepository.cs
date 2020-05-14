using _365Home.DataAccess.Data.Repository.IRepository;
using _365Home.Models;
using _365Home.Ultility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _365Home.DataAccess.Data.Repository
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        private readonly ApplicationDbContext _db;
        public LocationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Delete(Location location)
        {
            var objFromDb = _db.Location.FirstOrDefault(s => s.Id == location.Id);

            objFromDb.IsDeleted = true;

            _db.SaveChanges();
        }

        public void Update(Location location)
        {
            var objFromDb = _db.Location.FirstOrDefault(s => s.Id == location.Id);

            objFromDb.Name = location.Name;
            objFromDb.Address = location.Address;
            objFromDb.ProvinceId = location.ProvinceId;
            objFromDb.DistrictId = location.DistrictId;
            objFromDb.WardId = location.WardId;
            objFromDb.Area = location.Area;
            objFromDb.Description = location.Description;
            objFromDb.Price = location.Price;
            //objFromDb.ImageId = location.ImageId;
            objFromDb.LocationStatus = location.LocationStatus;
            objFromDb.IsPublic = location.IsPublic;
            objFromDb.LocationType = location.LocationType;
            objFromDb.ModifiedDateTime = location.ModifiedDateTime;

            _db.SaveChanges();
        }

        public void LockLocation(Location location)
        {
            var objFromDb = _db.Location.FirstOrDefault(s => s.Id == location.Id);
            objFromDb.LocationStatus = SD.LocationStatusLocked;
            _db.SaveChanges();
        }

        public void UnLockLocation(Location location)
        {
            var objFromDb = _db.Location.FirstOrDefault(s => s.Id == location.Id);
            objFromDb.LocationStatus = SD.LocationStatusVacant;
            _db.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetLocationStatusListForDropDown()
        {
            return _db.Location.Select(i => new SelectListItem()
            {
                Text = i.LocationStatus,
                Value = i.LocationStatus
            }).Distinct();
        }
    }
}
