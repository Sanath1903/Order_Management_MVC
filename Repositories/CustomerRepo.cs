

namespace InventoryBeginners.Repositories
{
    public class CustomerRepo : ICustomer
    {
        private readonly InventoryContext _context;
        public CustomerRepo(InventoryContext context) // will be passed by dependency injection.
        {
            _context = context;
        }

        public Customer Create(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return customer;
        }


        public Customer GetItem(int id)
        {
            Customer customer = _context.Customers.Where(s => s.Id == id).FirstOrDefault();
            return customer;
        }

        private List<Customer> DoSort(List<Customer> customers, string SortProperty, SortOrder sortOrder)
        {
            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    customers = customers.OrderBy(n => n.Name).ToList();
                else
                    customers = customers.OrderByDescending(n => n.Name).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    customers = customers.OrderBy(d => d.Code).ToList();
                else
                    customers = customers.OrderByDescending(d => d.Code).ToList();
            }

            return customers;
        }

        public PaginatedList<Customer> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {

            List<Customer> customers;

            if (SearchText != "" && SearchText != null)
            {
                customers = _context.Customers.Where(n => n.Name.Contains(SearchText) || n.Code.Contains(SearchText))
                    .ToList();
            }
            else
                customers = _context.Customers.ToList();

            customers = DoSort(customers, SortProperty, sortOrder);

            PaginatedList<Customer> retcustomers = new PaginatedList<Customer>(customers, pageIndex, pageSize);

            return retcustomers;

        }

        public bool IsCustomerCodeExists(string code)
        {
            
            int ct = _context.Customers.Where(s => s.Code.ToLower() == code.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;

        }
        public bool IsCustomerCodeExists(string code, int Id)
        {
            int ct = _context.Customers.Where(s => s.Code.ToLower() == code.ToLower() &&  s.Id!=Id).Count();
            if (ct > 0)
                return true;
            else
                return false;

        }
        public bool IsCustomerEmailExists(string email)
        {
            int ct = _context.Customers.Where(s => s.EmailId.ToLower() == email.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;

        }
        public bool IsCustomerEmailExists(string email, int Id)
        {
            int ct = _context.Customers.Where(s => s.EmailId.ToLower() == email.ToLower() && s.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsCustomerNameExists(string name)
        {
            int ct = _context.Customers.Where(s => s.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;

        }
        public bool IsCustomerNameExists(string name, int Id)
        {
            int ct = _context.Customers.Where(s => s.Name.ToLower() == name.ToLower() && s.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
