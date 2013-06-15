using System.Collections.Generic;
using SportsStore.Core.Contracts.Models;

namespace SportsStore.Web.UI.Models
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }
        public IList<Category> Categories { get; set; }
        public string ReturnUrl { get; set; }
    }
}