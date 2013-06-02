using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Core.Contracts.Models;

namespace SportsStore.Web.UI.Binders
{
    public class CartModelBinder :IModelBinder
    {
        private const string sessionKey = "Cart";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Contract.Requires<NullReferenceException>(controllerContext.HttpContext.Session != null, "Not in web context");

            // Attempt to get the Cart from the session
            Cart cart = (Cart)controllerContext.HttpContext.Session[sessionKey];

            // Create the Cart if there wasn't one in the session data
            if (cart == null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session[sessionKey] = cart;
            }

            return cart;
        }
    }
}