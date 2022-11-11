namespace InventoryBeginners.Data
{
    public class InventoryContext: IdentityDbContext
    {
        public InventoryContext(DbContextOptions options):base(options)
        {

        }


     

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }


        public virtual DbSet<OrderHeader> OrderHeaders  { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }



    }
}
