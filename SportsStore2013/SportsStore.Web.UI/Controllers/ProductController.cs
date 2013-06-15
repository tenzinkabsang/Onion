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
    public class ProductController : BaseProductController
    {
        public ProductController(IProductCore products)
            : base(products)
        {
        }

        public ActionResult Details(int id, string returnUrl)
        {
            Product product = _products.GetProductById(id);

            ProductDetail viewModel = new ProductDetail(product);

            ViewBag.ReturnUrl = returnUrl;

            return View(viewModel);
        }

    }
}
