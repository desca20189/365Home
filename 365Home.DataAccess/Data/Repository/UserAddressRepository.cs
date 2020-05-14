using _365Home.DataAccess.Data.Repository.IRepository;
using _365Home.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _365Home.DataAccess.Data.Repository
{
    public class UserAddressRepository : Repository<UserAddress>, IUserAddressRepository
    {
        private readonly ApplicationDbContext _db;
        public UserAddressRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(UserAddress userAddress)
        {
            var objFromDb = _db.UserAddress.Find(userAddress.Id);

            objFromDb.UserId = userAddress.UserId;
            objFromDb.PhoneNumber = userAddress.PhoneNumber;
            objFromDb.AddressName = userAddress.AddressName;
            objFromDb.Address = userAddress.Address;
            objFromDb.IsDefaultAddress = userAddress.IsDefaultAddress;

            _db.SaveChanges();
        }

        public void SetIsDefaultAddressToFalse(UserAddress userAddress)
        {
            var objFromDb = _db.UserAddress.Find(userAddress.Id);

            objFromDb.IsDefaultAddress = false;
            _db.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetUserAddressListForDropDownBaseOnUser(string userId)
        {
            return _db.UserAddress.Where(i=> i.UserId == userId).Select(i => new SelectListItem()
            {
                Text = i.AddressName,
                Value = i.Id.ToString()
            });
        }

        
    }
}
