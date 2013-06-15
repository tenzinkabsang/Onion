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
    public class AdminControllerTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            // Arrange
            Mock<IProductCore> mock = new Mock<IProductCore>();
            mock.Setup(m => m.GetProductsForCategoryName(null)).Returns(new List<Product>
                                                            {
                                                                new Product { Id = 1, Name = "P1" },
                                                                new Product { Id = 2, Name = "P2" },
                                                                new Product { Id = 3, Name = "P3" }
                                                            });

            AdminController target = new AdminController(mock.Object, null);

            // Act
            ProductsListViewModel viewModel = ((ViewResult)target.List(null)).ViewData.Model as ProductsListViewModel;
            List<Product> result = viewModel.Products.ToList();

            // Assert
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            // Arrange
            Mock<IProductCore> mock = new Mock<IProductCore>();
            mock.Setup(m => m.GetProductById(It.IsAny<int>()))
                .Returns(new Product { Id = 1, Name = "P1" });

            Mock<ICategoryCore> stubCategory = new Mock<ICategoryCore>();

            AdminController target = new AdminController(mock.Object, stubCategory.Object);

            // Act
            ProductEditViewModel result = ((ViewResult)target.Edit(1, null)).Model as ProductEditViewModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Product.Id);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Arrange
            Product product = new Product { Name = "TestProduct" };
            Mock<IProductCore> mock = new Mock<IProductCore>();
            mock.Setup(m => m.Save(product)).Returns(mock.Object);

            AdminController target = new AdminController(mock.Object, null);

            ProductEditViewModel model = new ProductEditViewModel { Product = product };

            // Act
            ActionResult result = target.Edit(model);

            // Assert
            mock.Verify(m => m.Save(product));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Arrange
            Mock<IProductCore> mock = new Mock<IProductCore>();

            AdminController target = new AdminController(mock.Object, null);

            Product product = new Product { Name = "TestProduct" };

            ProductEditViewModel model = new ProductEditViewModel { Product = product };

            target.ModelState.AddModelError("error", "error");

            // Act
            ActionResult result = target.Edit(model);

            // Assert
            mock.Verify(m => m.Save(product), Times.Never());
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
