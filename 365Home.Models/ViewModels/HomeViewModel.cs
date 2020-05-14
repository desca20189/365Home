using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _365Home.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }

        public IEnumerable<Service> ServiceList { get; set; }

        public IEnumerable<Location> LocationList { get; set; }

        public PaginatedList<Location> paginatedLocationList { get; set; }
    }
}
