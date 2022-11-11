
namespace InventoryBeginners.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ICustomer _repo;
        public CustomerController(ICustomer repo) // here the repository will be passed by the dependency injection.
        {
            _repo = repo;
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult IsEmailExists(string EmailId, int Id = 0)
        {
            bool isexists = _repo.IsCustomerEmailExists(EmailId, Id);

            if (isexists)
                return Json(data: false);
            else
                return Json(data: true);
        }



        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("code");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;
            PaginatedList<Customer> customers = _repo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);

            var pager = new PagerModel(customers.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;
            return View(customers);
        }


        public IActionResult Create()
        {
            Customer customer = new Customer();
            return View(customer);
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (_repo.IsCustomerCodeExists(customer.Code) == true)
                    errMessage = errMessage + " " + " Customer Code " + customer.Code + " Exists Already";

                if (_repo.IsCustomerNameExists(customer.Name) == true)
                    errMessage = errMessage + " " + " Customer Name " + customer.Name + " Exists Already";

                if (_repo.IsCustomerEmailExists(customer.EmailId) == true)
                    errMessage = errMessage + " " + " Customer Email " + customer.EmailId + " Exists Already";

                if (errMessage == "")
                {
                    customer = _repo.Create(customer);
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }
            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(customer);
            }
            else
            {
                TempData["SuccessMessage"] = "Customer " + customer.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id) //Read
        {
            Customer customer = _repo.GetItem(id);
            return View(customer);
        }

    }
}
