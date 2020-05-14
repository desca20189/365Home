using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace _365Home.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderHeaderId { get; set; }
        
        [ForeignKey("OrderHeaderId")]
        public OrderHeader OrderHeader { get; set; }

        //[Required]
        //public int ServiceId { get; set; }

        //[ForeignKey("ServiceId")]
        //public Service Service { get; set; }

        //[Required]
        //public string ServiceName { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public float Price { get; set; }

        [Required]
        public string LocationId { get; set; }

        [ForeignKey("LocationId")]
        public Location Location { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? RentTimeStartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? RentTimeEndDate { get; set; }

        //THIS IS THE USER CREATED THE LOCATION IN THE ORDER DETAIL
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public string AddressId { get; set; }

        [ForeignKey("AddressId")]
        public UserAddress UserAddress { get; set; }

        public string Status { get; set; }
    }
}
