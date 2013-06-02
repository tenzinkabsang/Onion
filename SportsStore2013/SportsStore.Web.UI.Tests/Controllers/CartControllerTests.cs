using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Core.Contracts;
using SportsStore.Core.Contracts.Models;
using SportsStore.Web.UI.Controllers;
using SportsStore.Web.UI.Models;

namespace SportsStore.Web.UI.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Arrange
            var mock = new Mock<IProductCore>();
            mock.Setup(m => m.GetProductFor(It.IsAny<int>())).Returns(new Product { Id = 1, Name = "P1" });
            Cart cart = new Cart();

            CartController target = new CartController(mock.Object);

            // Act
            target.AddToCart(cart, 1, 1, null);

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.First().Product.Id, 1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            // Arrange
            var mock = new Mock<IProductCore>();
            mock.Setup(m => m.GetProductFor(It.IsAny<int>())).Returns(new Product { Id = 1, Name = "P1" });
            Cart cart = new Cart();

            CartController target = new CartController(mock.Object);

            // Act
            RedirectToRouteResult result = target.AddToCart(cart, 1, 1, "myUrl");

            // Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Arrange
            Cart cart = new Cart();
            CartController target = new CartController(null);

            // Act
            CartIndexViewModel result = (CartIndexViewModel)((ViewResult)target.Index(cart, "myUrl")).ViewData.Model;

            // Assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Arrange
            var mock = new Mock<IProductCore>();
            var cart = new Cart();
            var shippingDetails = new ShippingDetails();
            var target = new CartController(mock.Object);

            // Act
            ViewResult result = (ViewResult)target.Checkout(cart, shippingDetails);

            // Assert
            mock.Verify(m => m.ProcessOrder(cart, shippingDetails), Times.Never());
            
            // - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            // - check that we are passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Arrange
            var mock = new Mock<IProductCore>();
            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            CartController target = new CartController(mock.Object);

            // add an error to the model
            target.ModelState.AddModelError("error", "error");

            // Act
            ViewResult result = (ViewResult)target.Checkout(cart, new ShippingDetails());

            // Assert
            mock.Verify(m => m.ProcessOrder(cart, It.IsAny<ShippingDetails>()), Times.Never());
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange
            var mock = new Mock<IProductCore>();
            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            CartController target = new CartController(mock.Object);

            // Act
            ViewResult result = (ViewResult)target.Checkout(cart, new ShippingDetails());

            // Assert
            mock.Verify(m => m.ProcessOrder(cart, It.IsAny<ShippingDetails>()), Times.Once());

            // check that the method is returning the Completed View
            Assert.AreEqual("Completed", result.ViewName);
            
            // check that we are passing a valid model to the view
            Assert.IsTrue(result.ViewData.ModelState.IsValid);
        }
    }
}
