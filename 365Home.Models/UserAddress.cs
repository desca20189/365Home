using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace _365Home.Models
{
    public class UserAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Display(Name = "Address Name")]
        public string AddressName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Address Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Address Details")]
        public string Address { get; set; }

        public bool IsDefaultAddress { get; set; }
    }
}
