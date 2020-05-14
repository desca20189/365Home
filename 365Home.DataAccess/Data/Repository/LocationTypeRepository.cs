using _365Home.DataAccess.Data.Repository.IRepository;
using _365Home.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _365Home.DataAccess.Data.Repository
{
    public class LocationTypeRepository : Repository<LocationType>, ILocationTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public LocationTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetLocationTypeListForDropDown()
        {
            return _db.LocationType.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }
    }
}
