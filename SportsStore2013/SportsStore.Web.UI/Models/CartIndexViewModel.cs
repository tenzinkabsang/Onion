using SportsStore.Core.Contracts.Models;

namespace SportsStore.Web.UI.Models
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}