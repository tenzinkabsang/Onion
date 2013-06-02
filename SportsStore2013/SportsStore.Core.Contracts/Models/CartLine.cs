namespace SportsStore.Core.Contracts.Models
{
    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal LineSubTotal
        {
            get
            {
                return Product.Price * Quantity;
            }
        }
    }
}