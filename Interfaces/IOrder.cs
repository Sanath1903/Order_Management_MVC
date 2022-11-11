namespace InventoryBeginners.Interfaces
{
    public interface IOrder
    {
        public string GetErrors();

        PaginatedList<OrderHeader> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
        OrderHeader GetItem(int id); // read particular item

        bool Create(OrderHeader OrderHeader);
        bool Edit(OrderHeader OrderHeader);
        bool Delete(OrderHeader OrderHeader);

        public bool IsOrderNumberExists(string orderNumber);
        public bool IsOrderNumberExists(string orderNumber, int Id);

   
        public string GetNewORDERNumber();


    }
}
