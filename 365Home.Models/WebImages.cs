using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace _365Home.Models
{
    public class WebImages
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public byte[] Picture { get; set; }

        public string LocationId  { get; set; }

        [ForeignKey("LocationId")]
        public Location Location { get; set; }
    }
}
