using _365Home.DataAccess.Data.Repository.IRepository;
using _365Home.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _365Home.DataAccess.Data.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void LockUser(string userId)
        {
            var userFromDB = _db.ApplicationUser.FirstOrDefault(u=>u.Id == userId);
            userFromDB.LockoutEnd = DateTime.Now.AddYears(100);
            _db.SaveChanges();
        }

        public void UnLockUser(string userId)
        {
            var userFromDB = _db.ApplicationUser.FirstOrDefault(u=>u.Id == userId);
            userFromDB.LockoutEnd = DateTime.Now;
            _db.SaveChanges();
        }

        public void Update(ApplicationUser user)
        {
            var userFromDB = _db.ApplicationUser.FirstOrDefault(u=>u.Id == user.Id);
            userFromDB.StreetAddress = user.StreetAddress;
            userFromDB.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            userFromDB.EmailConfirmed = user.EmailConfirmed;
            userFromDB.Name = user.Name;
            _db.SaveChanges();
        }

        public string CheckUserRole(ApplicationUser user)
        {
            var userFromDB = _db.ApplicationUser.FirstOrDefault(u => u.Id == user.Id);
            var role = _db.UserRoles.FirstOrDefault(x => x.UserId == user.Id);
            var roleName = _db.Roles.FirstOrDefault(x => x.Id == role.RoleId);
            return roleName.Name;
        }
    }
}
