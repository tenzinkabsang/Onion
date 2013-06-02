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
    public class CartController : Controller
    {
        private readonly IProductCore _products;

        public CartController(IProductCore products)
        {
            _products = products;
        }

        public ActionResult Index(Cart cart, string returnUrl)
        {
            var viewModel = new CartIndexViewModel
                                {
                                    Cart = cart,
                                    ReturnUrl = returnUrl
                                };

            return View(viewModel);
        }

        public RedirectToRouteResult AddToCart(Cart cart, int id, int quantity, string returnUrl)
        {
            Product product = _products.GetProductFor(id);

            if(product != null)
                cart.AddItem(product, quantity);

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int id, string returnUrl)
        {
            Product product = _products.GetProductFor(id);

            if(product != null)
                cart.RemoveLine(product);

            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }


        public ActionResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if(!cart.Lines.Any())
                ModelState.AddModelError(string.Empty, "Sorry, your cart is empty");

            if (!ModelState.IsValid)
                return View(shippingDetails);

            // If we got this far, order is ready to be processed.
            _products.ProcessOrder(cart, shippingDetails);
            cart.Clear();
            return View("Completed");
        }
    }
}
