using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Core.Contracts;
using SportsStore.Core.Contracts.Models;
using SportsStore.Web.UI.Controllers;
using SportsStore.Web.UI.HtmlHelpers;
using SportsStore.Web.UI.Models;

namespace SportsStore.Web.UI.Tests.Controllers
{
    [TestClass]
    public class ProductControllerTests
    {
        private List<Product> StubProducts()
        {
            return new List<Product>
                            {
                                new Product{ Id = 1, Name = "P1"},
                                new Product{ Id = 2, Name = "P2"},
                                new Product{ Id = 3, Name = "P3"},
                                new Product{ Id = 4, Name = "P4"},
                                new Product{ Id = 5, Name = "P5"}
                            };
        }
        [TestMethod]
        public void Can_Paginate()
        {
            // Arrange
            Mock<IProductCore> mock = new Mock<IProductCore>();
            mock.Setup(m => m.GetProductsForCategoryName(It.IsAny<string>())).Returns(StubProducts());
            ProductController target = new ProductController(mock.Object) { PageSize = 3 };

            // Act
            ProductsListViewModel result = (ProductsListViewModel)((ViewResult)target.List(null, 2)).Model;

            // Assert
            List<Product> products = result.Products.ToList();
            Assert.IsTrue(products.Count == 2);
            Assert.AreEqual(products[0].Name, "P4");
            Assert.AreEqual(products[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrange - define an HTML helper - we need to do this in order to apply the extension method
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);


            // Assert
            Assert.AreEqual(result.ToString(),
                @"<ul>" +
                    @"<li><a href=""Page1"">1</a></li>" +
                    @"<li><a class=""selected"" href=""Page2"">2</a></li>" +
                    @"<li><a href=""Page3"">3</a></li>" +
                @"</ul>");
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            Mock<IProductCore> mock = new Mock<IProductCore>();
            mock.Setup(m => m.GetProductsForCategoryName(It.IsAny<string>())).Returns(StubProducts());

            ProductController target = new ProductController(mock.Object)
            {
                PageSize = 3
            };

            // Act
            ProductsListViewModel result = (ProductsListViewModel)((ViewResult)target.List(null, 2)).Model;


            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(2, pageInfo.CurrentPage);
            Assert.AreEqual(3, pageInfo.ItemsPerPage);
            Assert.AreEqual(5, pageInfo.TotalItems);
            Assert.AreEqual(2, pageInfo.TotalPages);
        }

    }
}
