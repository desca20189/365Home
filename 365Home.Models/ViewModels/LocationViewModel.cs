using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace _365Home.Models.ViewModels
{
    public class LocationViewModel
    {
        public Location Location { get; set; }
        public IEnumerable<SelectListItem> ProvinceList { get; set; }

        public IEnumerable<SelectListItem> DistrictList { get; set; }

        public IEnumerable<SelectListItem> WardList { get; set; }

        public IEnumerable<SelectListItem> LocationTypeList { get; set; }

        public IEnumerable<SelectListItem> LocationStatusList { get; set; }

        public ICollection<WebImages> ImageList { get; set; }
    }
}
