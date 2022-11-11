

namespace InventoryBeginners.Controllers
{
  
    public class ProductController : Controller
    {

        private readonly IWebHostEnvironment _webHost;

       




      
        private readonly IProduct _productRepo;
        public ProductController(IProduct productrepo, IWebHostEnvironment webHost) // here the repository will be passed by the dependency injection.
        {

            _webHost = webHost;

            _productRepo = productrepo;
          


        }


        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();

            sortModel.AddColumn("Code");
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            //sortModel.AddColumn("Cost");
            sortModel.AddColumn("Price");
            sortModel.AddColumn("Unit");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<Product> products = _productRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);



            var pager = new PagerModel(products.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(products);
        }


   

        public IActionResult Create()
        {
            Product product = new Product();

           // PopulateViewbags();


            return View(product);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (product.Description.Length < 4 || product.Description == null)
                    errMessage = "Product Description Must be atleast 4 Characters";



                if (_productRepo.IsItemCodeExists(product.Code) == true)
                    errMessage = errMessage + " " + " Product Code " + product.Code + " Exists Already";



                if (_productRepo.IsItemExists(product.Name) == true)
                    errMessage = errMessage + " " + " Product Name " + product.Name + " Exists Already";

                if (errMessage == "")
                {

                    string uniqueFileName = GetUploadedFileName(product);
                    product.PhotoUrl = uniqueFileName;


                    product = _productRepo.Create(product);
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
               // PopulateViewbags();       
                return View(product);

                //return RedirectToAction(nameof(Create));
            }
            else
            {
                TempData["SuccessMessage"] = "Product " + product.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(string id) //Read
        {
            Product product = _productRepo.GetItem(id);
            return View(product);
        }

        private string GetUploadedFileName(Product product)
        {
            string uniqueFileName = null;

            if (product.ProductPhoto != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ProductPhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.ProductPhoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        [AcceptVerbs("Get","Post")]
        public JsonResult IsProductCodeValid(string Code,string Name="")
        {

            bool isExists = _productRepo.IsItemCodeExists(Code,Name);

            if (isExists)
                return Json(data: false);
            else
                return Json(data: true);
        }


        [AcceptVerbs("Get", "Post")]
        public JsonResult IsProductNameValid(string Name,string Code="")
        {

            bool isExists = _productRepo.IsItemExists(Name,Code);

            if (isExists)
                return Json(data: false);
            else
                return Json(data: true);
        }




    }
}
