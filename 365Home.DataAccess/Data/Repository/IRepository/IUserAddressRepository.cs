using _365Home.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace _365Home.DataAccess.Data.Repository.IRepository
{
    public interface IUserAddressRepository : IRepository<UserAddress>
    {
        public void Update(UserAddress userAddress);

        public void SetIsDefaultAddressToFalse(UserAddress userAddress);

        IEnumerable<SelectListItem> GetUserAddressListForDropDownBaseOnUser(string userId);
    }
}
