using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace _365Home.Models
{
    public class District
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string LatiLongTude { get; set; }
		[Required]
		public int ProvinceId { get; set; }
		[ForeignKey("ProvinceId")]
		public Province Province { get; set; }
		public int SortOrder { get; set; }
		public bool IsPublished { get; set; }
		public bool IsDeleted { get; set; }
	}
}
