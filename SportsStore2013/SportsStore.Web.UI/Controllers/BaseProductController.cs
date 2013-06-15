using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SportsStore.Core.Contracts;
using SportsStore.Core.Contracts.Models;
using SportsStore.Web.UI.Models;

namespace SportsStore.Web.UI.Controllers
{
    public class BaseProductController : Controller
    {
        protected readonly IProductCore _products;

        public int PageSize { get; set; }

        public BaseProductController(IProductCore products, int pageSize = 4)
        {
            _products = products;
            PageSize = pageSize; // get it from config
        }

        public ActionResult List(string category, int page = 1)
        {
            IList<Product> products = _products.GetProductsForCategoryName(category);

            var productsToDisplay = products
                .OrderBy(p => p.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            PagingInfo pagingInfo = new PagingInfo
                                        {
                                            CurrentPage = page,
                                            ItemsPerPage = PageSize,
                                            TotalItems = products.Count
                                        };
            ProductsListViewModel viewModel = new ProductsListViewModel
                                                  {
                                                      Products = productsToDisplay,
                                                      PagingInfo = pagingInfo,
                                                      CurrentCategory = category
                                                  };
            return View(viewModel);
        }
    }
}