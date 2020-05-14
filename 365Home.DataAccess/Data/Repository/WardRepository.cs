using _365Home.DataAccess.Data.Repository.IRepository;
using _365Home.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _365Home.DataAccess.Data.Repository
{
    public class WardRepository : Repository<Ward>, IWardRepository
    {
        private readonly ApplicationDbContext _db;
        public WardRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetWardListForDropDown()
        {
            return _db.Ward.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public IEnumerable<SelectListItem> GetWardListForDropDownByDistrictId(int? districtId)
        {
            IEnumerable<SelectListItem> wards = _db.Ward
                        .OrderBy(n => n.Name)
                        .Where(n => n.DistrictId == districtId)
                        .Select(n =>
                           new SelectListItem
                           {
                               Value = n.Id.ToString(),
                               Text = n.Name
                           }).ToList();
            return new SelectList(wards, "Value", "Text");
        }
    }
}
