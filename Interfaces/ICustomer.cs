

namespace InventoryBeginners.Interfaces
{
    public interface ICustomer
    {
        PaginatedList<Customer> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        Customer GetItem(int id); // read particular item

        Customer Create(Customer customer);
        public bool IsCustomerNameExists(string name);
        public bool IsCustomerNameExists(string name, int Id);

        public bool IsCustomerCodeExists(string code);
        public bool IsCustomerCodeExists(string code, int Id);

        public bool IsCustomerEmailExists(string email);
        public bool IsCustomerEmailExists(string email, int Id);
    }
}
