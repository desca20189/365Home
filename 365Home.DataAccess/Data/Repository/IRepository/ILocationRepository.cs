using _365Home.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace _365Home.DataAccess.Data.Repository.IRepository
{
    public interface ILocationRepository : IRepository<Location>
    {
        void Delete(Location location);

        void Update(Location location);

        void LockLocation(Location location);
        
        void UnLockLocation(Location location);

        IEnumerable<SelectListItem> GetLocationStatusListForDropDown();
    }

}
