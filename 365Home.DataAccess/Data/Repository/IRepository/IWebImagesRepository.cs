using _365Home.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _365Home.DataAccess.Data.Repository.IRepository
{
    public interface IWebImagesRepository : IRepository<WebImages>
    {
        void Delete(WebImages image);
    }
}
