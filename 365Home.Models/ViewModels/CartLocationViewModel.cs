using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace _365Home.Models.ViewModels
{
    public class CartLocationViewModel
    {
        public string locationId { get; set; }

        [Required(ErrorMessage = "Please pick a Renting Start Date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? RentTimeStartDate { get; set; }

        [Required(ErrorMessage = "Please pick a Renting End Date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? RentTimeEndDate { get; set; }

        public Location Location { get; set; }

        public bool LocationBooked { get; set; }

        //public IList<UserAddress> UserAddressList { get; set; }

        //[Required(ErrorMessage = "Please select an address.")]
        //public string AddressId { get; set; }

        //public IEnumerable<SelectListItem> UserAddressesList { get; set; }

    }
}
