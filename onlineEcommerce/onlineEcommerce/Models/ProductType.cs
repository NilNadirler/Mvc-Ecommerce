using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace onlineEcommerce.Models
{
    public class ProductType
    {
        [Key]
        public int Id { get; set; }

        
        [Display(Name="Product Type")]
        [Required]
        public string? ProductTypeName { get; set; }
    }
}
