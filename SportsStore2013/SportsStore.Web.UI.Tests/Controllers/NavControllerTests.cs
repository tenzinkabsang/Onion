using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Core.Contracts;
using SportsStore.Web.UI.Controllers;

namespace SportsStore.Web.UI.Tests.Controllers
{
    [TestClass]
    public class NavControllerTests
    {
        [TestMethod]
        public void Can_Create_Categories()
        {
            // Arrange
            Mock<ICategoryCore> mock = new Mock<ICategoryCore>();
            mock.Setup(m => m.GetCategoryNames()).Returns(new List<string>()
                                                              {
                                                                  "Apples",
                                                                  "Plums",
                                                                  "Oranges"
                                                              });

            NavController target = new NavController(mock.Object);

            // Act
            var result = ((IList<string>)target.Menu().Model);

            // Assert
            Assert.AreEqual(result.Count, 3);
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Arrange
            var mock = new Mock<ICategoryCore>();
            mock.Setup(m => m.GetCategoryNames()).Returns(new List<string>
                                                              {
                                                                  "Apples",
                                                                  "Plums",
                                                                  "Oranges"
                                                              });

            var target = new NavController(mock.Object);
            string categoryToSelect = "Plums";

            // Act
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Assert
            Assert.AreEqual(categoryToSelect, result);
        }
    }
}
