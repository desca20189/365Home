using _365Home.DataAccess.Data.Repository.IRepository;
using _365Home.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _365Home.DataAccess.Data.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void ChangeOrderStatus(int orderDetaildId, string status)
        {
            var orderFromDb = _db.OrderDetails.FirstOrDefault(o => o.Id == orderDetaildId);
            orderFromDb.Status = status;
            _db.SaveChanges();
        }

        public OrderDetails FindBaseOnOrderHeaderId(int orderHeaderId)
        {
            if(orderHeaderId != 0)
            {
                var orderFromDb = _db.OrderDetails.FirstOrDefault(o => o.OrderHeaderId == orderHeaderId);
                return orderFromDb;
            }
            return null;
        }

        public OrderDetails FindBaseOnOrderHeaderIdAndStatus(int orderHeaderId, string status)
        {
            if (orderHeaderId != 0)
            {
                var orderFromDb = _db.OrderDetails.FirstOrDefault(o => o.OrderHeaderId == orderHeaderId && o.Status == status);
                return orderFromDb;
            }
            return null;
        }

        public OrderDetails FindBaseOnLocationIdAndStatusAndStillActive(string locationId, DateTime date, string status)
        {
            if(locationId != null)
            {
                var orderDetailFromDb = _db.OrderDetails.FirstOrDefault(o => o.LocationId == locationId 
                && o.RentTimeEndDate >= date 
                && o.Status == status);
                
                return orderDetailFromDb;
            }
            else
            {
                return null;
            }
        }
    }
}
