namespace InventoryBeginners.Repositories
{
    public class OrderRepo : IOrder
    {
        private string _errors = "";

        public string GetErrors()
        {
            return _errors;
        }


        private readonly InventoryContext _context; // for connecting to efcore.
        public OrderRepo(InventoryContext context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public bool Create(OrderHeader OrderHeader)
        {
            bool retVal = false;
            _errors = "";

            try
            {
                _context.OrderHeaders.Add(OrderHeader);
                _context.SaveChanges();
                retVal = true;
            }
            catch (Exception ex)
            {
                _errors = "Create Failed - Sql Exception Occured , Error Info : " + ex.Message;
            }
            return retVal;
        }

        public bool Delete(OrderHeader OrderHeader)
        {
            bool retVal = false;
            _errors = "";

            try
            {
                _context.Attach(OrderHeader);
                _context.Entry(OrderHeader).State = EntityState.Deleted;
                _context.SaveChanges();
                retVal = true;
            }
            catch (Exception ex)
            {
                _errors = "Delete Failed - Sql Exception Occured , Error Info : " + ex.Message;
            }
            return retVal;
        }


        public bool Edit(OrderHeader OrderHeader)
        {
            bool retVal = false;
            _errors = "";

            try
            {

                List<OrderDetail> orderDetails = _context.OrderDetails.Where(d => d.PoId == OrderHeader.Id).ToList();
                _context.OrderDetails.RemoveRange(orderDetails);
                _context.SaveChanges();

                _context.Attach(OrderHeader);
                _context.Entry(OrderHeader).State = EntityState.Modified;
                _context.OrderDetails.AddRange(OrderHeader.OrderDetails);
                _context.SaveChanges();


                retVal = true;
            }
            catch (Exception ex)
            {
                _errors = "Update Failed - Sql Exception Occured , Error Info : " + ex.Message;
            }
            return retVal;
        }




        private List<OrderHeader> DoSort(List<OrderHeader> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "ordernumber")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.OrderNumber).ToList();
                else
                    items = items.OrderByDescending(n => n.OrderNumber).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(d => d.Id).ToList();
                else
                    items = items.OrderBy(d => d.Id).ToList();
            }

            return items;
        }

        public PaginatedList<OrderHeader> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<OrderHeader> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.OrderHeaders.Where(n => n.OrderNumber.Contains(SearchText))
                    .Include(s => s.Customer)
                    .ToList();
            }
            else
                items = _context.OrderHeaders
                   .Include(s => s.Customer)
                   .ToList();




            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<OrderHeader> retItems = new PaginatedList<OrderHeader>(items, pageIndex, pageSize);

            return retItems;
        }

        public OrderHeader GetItem(int Id)
        {
            OrderHeader item = _context.OrderHeaders.Where(i => i.Id == Id)
                     .Include(d => d.OrderDetails)
                     .ThenInclude(i => i.Product)
                     .FirstOrDefault();

            item.OrderDetails.ForEach(p => p.Price = p.Product.Price);
            item.OrderDetails.ForEach(p => p.Description = p.Product.Description);
            item.OrderDetails.ForEach(p => p.Total = p.Quantity * p.Price);

            return item;
        }


        public bool IsOrderNumberExists(string ordernumber)
        {
            int ct = _context.OrderHeaders.Where(n => n.OrderNumber.ToLower() == ordernumber.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsOrderNumberExists(string ordernumber, int Id = 0)
        {
            if (Id == 0)
                return IsOrderNumberExists(ordernumber);

            var poHeads = _context.OrderHeaders.Where(n => n.OrderNumber == ordernumber).Max(cd => cd.Id);
            if (poHeads == 0 || poHeads == Id)
                return false;
            else
                return true;
        }



        public string GetNewORDERNumber()
        {

            string ordernumber = "";
            var LastOrderNumber = _context.OrderHeaders.Max(cd => cd.OrderNumber);

            if (LastOrderNumber == null)
                ordernumber = "OR00001";
            else
            {
                int lastdigit = 1;
                int.TryParse(LastOrderNumber.Substring(2, 5).ToString(), out lastdigit);


                ordernumber = "OR" + (lastdigit + 1).ToString().PadLeft(5, '0');
            }


            return ordernumber;


        }

    }
}
