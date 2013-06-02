using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Core.Contracts.Models;

namespace SportsStore.Core.Tests.Models
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Arrange
            Product p1 = new Product { Id = 1, Name = "P1" };
            Product p2 = new Product { Id = 2, Name = "P2" };

            Cart target = new Cart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            List<CartLine> results = target.Lines.ToList();

            // Assert
            Assert.AreEqual(results.Count, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Arrange
            Product p1 = new Product { Id = 1, Name = "P1" };
            Product p2 = new Product { Id = 2, Name = "P2" };

            Cart target = new Cart();


            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);

            List<CartLine> results = target.Lines.ToList();

            // Assert
            Assert.AreEqual(results.Count, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Arrange
            Product p1 = new Product { Id = 1, Name = "P1" };
            Product p2 = new Product { Id = 2, Name = "P2" };
            Product p3 = new Product { Id = 3, Name = "P3" };

            Cart target = new Cart();
            

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            target.RemoveLine(p2);

            // Assert
            Assert.IsTrue(target.Lines.All(x => x.Product != p2));
            Assert.AreEqual(2, target.Lines.Count());
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Arrange
            Product p1 = new Product { Id = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { Id = 2, Name = "P2", Price = 50M };

            Cart target = new Cart();
            
            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);

            decimal result = target.ComputeTotalValue();

            // Assert
            Assert.AreEqual(450M, result);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Arrange
            Product p1 = new Product { Id = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { Id = 2, Name = "P2", Price = 50M };

            Cart target = new Cart();
            
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            // Act
            target.Clear();

            // Assert
            Assert.IsFalse(target.Lines.Any());
        }
    }
}
