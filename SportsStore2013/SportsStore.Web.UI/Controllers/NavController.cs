using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Core.Contracts;

namespace SportsStore.Web.UI.Controllers
{
    public class NavController : Controller
    {
        private readonly ICategoryCore _categories;

        public NavController(ICategoryCore categories)
        {
            _categories = categories;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = _categories.GetCategoryNames();

            return PartialView(categories);
        }

        public PartialViewResult AdminMenu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = _categories.GetCategoryNames();

            return PartialView(categories);
        }
    }
}
