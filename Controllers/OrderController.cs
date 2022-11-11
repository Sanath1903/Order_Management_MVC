using Microsoft.AspNetCore.Mvc;

namespace InventoryBeginners.Controllers
{
  
    public class OrderController : Controller
    {

        private readonly IOrder _Repo;

        private readonly IProduct _ProductRepo;


        private readonly ICustomer _CustomerRepo;




        public OrderController(IOrder repo, IProduct productRepo, ICustomer customerRepo)
        {
            _Repo = repo;
            _ProductRepo = productRepo;

            _CustomerRepo = customerRepo;


        }

        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Id");
            sortModel.AddColumn("OrderNumber");
            sortModel.AddColumn("OrderDate");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<OrderHeader> items = _Repo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);


            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;
            return View(items);
        }

        public IActionResult Create()
        {
            OrderHeader item = new OrderHeader();
            item.OrderDetails.Add(new OrderDetail() { Id = 1 });
            ViewBag.ProductList = GetProducts();
            ViewBag.CustomerList = GetCustomers();



            item.OrderNumber = _Repo.GetNewORDERNumber();


            //GetSuppliers()




            return View(item);
        }

        [HttpPost]
        public IActionResult Create(OrderHeader item)
        {
            item.OrderDetails.RemoveAll(a => a.Quantity == 0);


            bool bolret = false;
            string errMessage = "";
            try
            {
                bolret = _Repo.Create(item);
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }


            if (bolret == false)
            {
                errMessage = errMessage + " " + _Repo.GetErrors();

                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(item);
            }
            else
            {
                TempData["SuccessMessage"] = "" + item.OrderNumber + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }


        public IActionResult Details(int id)
        {
            OrderHeader item = _Repo.GetItem(id);

            ViewBag.ProductList = GetProducts();
            ViewBag.CustomerList = GetCustomers();


            return View(item);
        }

        private List<SelectListItem> GetProducts()
        {
            var lstProducts = new List<SelectListItem>();

            PaginatedList<Product> products = _ProductRepo.GetItems("Name", SortOrder.Ascending, "", 1, 1000);

            lstProducts = products.Select(ut => new SelectListItem()
            {
                Value = ut.Code.ToString(),
                Text = ut.Name

            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Product----"
            };

            lstProducts.Insert(0, defItem);

            return lstProducts;
        }


        private List<SelectListItem> GetCustomers()
        {
            var lstCustomers = new List<SelectListItem>();

            PaginatedList<Customer> customers = _CustomerRepo.GetItems("Name", SortOrder.Ascending, "", 1, 1000);

            lstCustomers = customers.Select(sp => new SelectListItem()
            {
                Value = sp.Id.ToString(),
                Text = sp.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Customer ----"
            };

            lstCustomers.Insert(0, defItem);

            return lstCustomers;
        }



        public IActionResult Edit(int id)
        {
            OrderHeader item = _Repo.GetItem(id);

            ViewBag.ProductList = GetProducts();
            ViewBag.CustomerList = GetCustomers();



            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(OrderHeader item)
        {
            item.OrderDetails.RemoveAll(a => a.Quantity == 0);


            bool bolret = false;
            string errMessage = "";
            try
            {
                bolret = _Repo.Edit(item);
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }


            if (bolret == false)
            {
                errMessage = errMessage + " " + _Repo.GetErrors();

                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(item);
            }
            else
            {
                TempData["SuccessMessage"] = "" + item.OrderNumber + " Modified Successfully";
                return RedirectToAction(nameof(Index));
            }


        }

        public IActionResult Delete(int id)
        {
            OrderHeader item = _Repo.GetItem(id);

            ViewBag.ProductList = GetProducts();
            ViewBag.CustomerList = GetCustomers();



            return View(item);
        }

        [HttpPost]
        public IActionResult Delete(OrderHeader item)
        {
            item.OrderDetails.RemoveAll(a => a.Quantity == 0);


            bool bolret = false;
            string errMessage = "";
            try
            {
                bolret = _Repo.Delete(item);
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }


            if (bolret == false)
            {
                errMessage = errMessage + " " + _Repo.GetErrors();

                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(item);
            }
            else
            {
                TempData["SuccessMessage"] = "" + item.OrderNumber + " Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
