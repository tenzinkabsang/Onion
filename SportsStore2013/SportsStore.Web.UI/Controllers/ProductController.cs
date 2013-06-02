using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Core.Contracts;
using SportsStore.Core.Contracts.Models;
using SportsStore.Web.UI.Models;

namespace SportsStore.Web.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductCore _products;

        public int PageSize = 4;

        public ProductController(IProductCore products)
        {
            _products = products;
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

        public ActionResult Details(int id, string returnUrl)
        {
            Product product = _products.GetProductFor(id);

            ProductDetail viewModel = new ProductDetail(product);

            ViewBag.ReturnUrl = returnUrl;

            return View(viewModel);
        }

    }
}
