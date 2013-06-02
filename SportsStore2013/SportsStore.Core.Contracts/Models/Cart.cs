using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Core.Contracts.Models
{
    public class Cart
    {
        private readonly List<CartLine> _lineCollection = new List<CartLine>();

        public IEnumerable<CartLine> Lines
        {
            get { return _lineCollection; }
        } 

        public void AddItem(Product product, int quantity)
        {
            CartLine line = _lineCollection.FirstOrDefault(p => p.Product.Id == product.Id);

            if (line == null)
                _lineCollection.Add(new CartLine { Product = product, Quantity = quantity });
            else
                line.Quantity += quantity;
        }

        public void RemoveLine(Product product)
        {
            _lineCollection.RemoveAll(x => x.Product.Id == product.Id);
        }

        public decimal ComputeTotalValue()
        {
            return _lineCollection.Sum(x => x.LineSubTotal);
        }

        public void Clear()
        {
            _lineCollection.Clear();
        }

        
    }
}
