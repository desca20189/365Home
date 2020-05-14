using System;
using System.Collections.Generic;
using System.Text;

namespace _365Home.Models.ViewModels
{
    public class CartViewModel
    {
        public IList<Service> ServiceList { get; set; }

        public IList<Location> LocationList { get; set; }

        public OrderHeader OrderHeader { get; set; }

        public CartLocationViewModel CartLocationViewModel { get; set; }

    }
}
