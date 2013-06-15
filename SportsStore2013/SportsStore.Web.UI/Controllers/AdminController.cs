using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Core.Contracts;
using SportsStore.Core.Contracts.Models;
using SportsStore.Web.UI.Models;
using SportsStore.Web.UI.Extensions;

namespace SportsStore.Web.UI.Controllers
{
    public class AdminController : BaseProductController
    {
        private readonly ICategoryCore _categories;

        public AdminController(IProductCore products, ICategoryCore categories)
            : base(products, pageSize: 10)
        {
            _categories = categories;
        }

        public ActionResult Edit(int id, string returnUrl)
        {
            Product product = _products.GetProductById(id);

            IList<Category> allCategories = _categories.GetAllCategories();

            var viewModel = new ProductEditViewModel
                                {
                                    Product = product,
                                    Categories = allCategories,
                                    ReturnUrl = returnUrl
                                };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(ProductEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                _products.Save(model.Product)
                         .Commit();

                TempData.AddMessage(MessageType.Success, string.Format("{0} has been saved", model.Product.Name));

                return RedirectToAction("List", new { category = (string)null });
            }
            
            //  there is something wrong with the data values
            return View(model);
        }

        public ActionResult Create(string returnUrl)
        {
            IList<Category> categories = _categories.GetAllCategories();
            var viewModel = new ProductEditViewModel { Product = new Product(), Categories = categories, ReturnUrl = returnUrl };
            return View("Edit", viewModel);
        }


        public ActionResult RefreshCache(string category)
        {
            _products.RefreshProductCache();
            return RedirectToAction("List", new { category });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Contract.Requires<ArgumentException>(id > 0, "Invalid productId");

            _products.Delete(id).Commit();

            return RedirectToAction("List", new { category = (string)null });
        }
    }
}
