namespace InventoryBeginners.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string OrderNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; } = DateTime.Now.Date;


        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; private set; }

        

        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "smallmoney")]
        [Required]
        public decimal ExchangeRate { get; set; }

        [Column(TypeName = "smallmoney")]
        [Required]
        public decimal DiscountPercent { get; set; }


        [Required]
        [MaxLength(15)]
        public string QuotationNo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime QuotationDate { get; set; }=  DateTime.Now.Date;



        [Required]
        [MaxLength(500)]
        public string PaymentTerms { get; set; } = " ";

        [Required]
        [MaxLength(500)]
        public string Remarks { get; set; } = " ";

        public virtual List<OrderDetail> OrderDetails { get; set; } =new List<OrderDetail>();        


    }
}
