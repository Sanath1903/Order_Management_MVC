namespace InventoryBeginners.Models
{
    public class Product
    {
        [Remote("IsProductCodeValid","Product", AdditionalFields ="Name", ErrorMessage ="Product Code Exists Already")]
        [Key]
        [StringLength(6)]
        public string Code { get; set; }

        [Remote("IsProductNameValid", "Product", AdditionalFields = "Code", ErrorMessage = "Product Name Exists Already")]
        [Required]
        [StringLength(75)]
        public String Name { get; set; }

        [Required]
        [StringLength(255)]
        public String Description { get; set; }

        //[Required]         
        //[Column(TypeName ="smallmoney")]
        //public decimal Cost { get; set; }

        [Required]
        [Column(TypeName = "smallmoney")]
        public decimal Price { get; set; }

        public string PhotoUrl { get; set; } = "noimage.png";

        [Display(Name = "Product Photo")]
        [NotMapped]
        public IFormFile ProductPhoto { get; set; }


    }
}
