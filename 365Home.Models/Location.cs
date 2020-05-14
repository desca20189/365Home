using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace _365Home.Models
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
        
        [Required]
        [Display(Name = "Province")]
        public int ProvinceId { get; set; }

        [ForeignKey("ProvinceId")]
        public Province Province { get; set; }

        [Required]
        [Display(Name = "District")]
        public int DistrictId { get; set; }

        [ForeignKey("DistrictId")]
        public District District { get; set; }

        [Display(Name = "Ward")]
        public int? WardId { get; set; }

        public Ward Ward { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime ModifiedDateTime { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Area { get; set; }

        public string Description { get; set; }
        
        [Required]
        [Display(Name = "Property Type")]
        public string LocationTypeId { get; set; }

        [ForeignKey("LocationTypeId")]
        public LocationType LocationType { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "Property Images")]
        public ICollection<WebImages> ImageList { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public float Price { get; set; }

        [Display(Name = "Property Status")]
        public string LocationStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime? RentTimeStartDate { get; set; }

         [DataType(DataType.Date)]
        public DateTime? RentTimeEndDate { get; set; }

        public bool IsPublic { get; set; }

        public DateTime? PublicationDate { get; set; }
    }
}
