using _365Home.DataAccess.Data.Repository.IRepository;
using _365Home.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _365Home.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Frequency = new FrequencyRepository(_db);
            Service = new ServiceRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
            User = new UserRepository(_db);
            StoreProcedureCall = new StoreProcedureCall(_db);
            Location = new LocationRepository(_db);
            Province = new ProvinceRepository(_db);
            District = new DistrictRepository(_db);
            Ward = new WardRepository(_db);
            LocationType = new LocationTypeRepository(_db);
            UserAddress = new UserAddressRepository(_db);
            WebImages = new WebImagesRepository(_db);
        }

        public ICategoryRepository Category { get; private set; }

        public IFrequencyRepository Frequency { get; private set; }

        public IServiceRepository Service { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }

        public IOrderDetailsRepository OrderDetails { get; private set; }

        public IUserRepository User { get; private set; }

        public IStoreProcedureCall StoreProcedureCall { get; private set; }

        public ILocationRepository Location { get; private set; }

        public IProvinceRepository Province { get; private set; }

        public IDistrictRepository District { get; private set; }

        public IWardRepository Ward { get; private set; }

        public ILocationTypeRepository LocationType { get; private set; }

        public IUserAddressRepository UserAddress { get; private set; }

        public IWebImagesRepository WebImages { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
