using _365Home.DataAccess.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _365Home.DataAccess.Data.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }

        IFrequencyRepository Frequency { get; }
        IServiceRepository Service { get; }

        IOrderDetailsRepository OrderDetails { get; }

        IOrderHeaderRepository OrderHeader { get; }

        IUserRepository User { get; }

        IStoreProcedureCall StoreProcedureCall { get; }

        ILocationRepository Location { get; }

        IProvinceRepository Province { get; }

        IDistrictRepository District { get; }

        IWardRepository Ward { get; }

        ILocationTypeRepository LocationType { get; }

        IUserAddressRepository UserAddress { get; }

        IWebImagesRepository WebImages { get; }

        void Save();
    }
}
