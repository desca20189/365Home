using _365Home.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace _365Home.DataAccess.Data.Repository.IRepository
{
    public interface IOrderDetailsRepository : IRepository<OrderDetails>
    {
        void ChangeOrderStatus(int orderDetailId, string status);

        OrderDetails FindBaseOnOrderHeaderId(int orderHeaderId);

        OrderDetails FindBaseOnOrderHeaderIdAndStatus(int orderHeaderId, string status);

        OrderDetails FindBaseOnLocationIdAndStatusAndStillActive(string locationId, DateTime date, string status);
    }
}
