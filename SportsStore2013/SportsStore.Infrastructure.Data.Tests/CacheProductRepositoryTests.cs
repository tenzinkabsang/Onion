using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Core.Contracts.Models;
using SportsStore.Core.Contracts.Repositories;
using SportsStore.Infrastructure.Data.Interfaces;

namespace SportsStore.Infrastructure.Data.Tests
{
    [TestClass]
    public class CacheProductRepositoryTests
    {
        [TestMethod]
        public void GetAll_WhenProductsInCache_ShouldNotCallActualRepository()
        {
            // Arrange
            var products = new List<Product>
                               {
                                   new Product { Id = 1, Name = "p1" },
                                   new Product { Id = 2, Name = "p2" }
                               };

            var mock = new Mock<IRepository<Product>>();
            var stubCache = new Mock<ICache>();
            stubCache.Setup(c => c.Get<IList<Product>>(It.IsAny<string>())).Returns(products);

            CachedProductRepository target = new CachedProductRepository(mock.Object, stubCache.Object);


            // Act
            var result = target.GetAll();

            // Assert
            mock.Verify(m => m.GetAll(), Times.Never());
        }

        [TestMethod]
        public void GetAll_WhenProductsNotInCache_ShouldCallActualRepository()
        {
            // Arrange
            var mock = new Mock<IRepository<Product>>();
            var stubCache = new Mock<ICache>();

            CachedProductRepository target = new CachedProductRepository(mock.Object, stubCache.Object);


            // Act
            var result = target.GetAll();

            // Assert
            mock.Verify(m => m.GetAll(), Times.Once());
        }

    }
}
