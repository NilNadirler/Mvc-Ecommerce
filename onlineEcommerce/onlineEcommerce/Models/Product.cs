using OnlineShop.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace onlineEcommerce.Models
{
    public class Product

    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        [Display(Name = "Product Color")]
        public string ProductColor { get; set; }
        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }
        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public virtual ProductType? ProductType { get; set; }
        [Display(Name="Special Tag")]
        public int SpeacialTagId { get; set; }
        [ForeignKey("SpecialTagId")]
        public virtual SpecialTag? SpecialTag { get; set; }
    }
}
