using _365Home.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace _365Home.DataAccess.Data.Repository.IRepository
{
    public interface ILocationTypeRepository : IRepository<LocationType>
    {
        IEnumerable<SelectListItem> GetLocationTypeListForDropDown();
    }
}
