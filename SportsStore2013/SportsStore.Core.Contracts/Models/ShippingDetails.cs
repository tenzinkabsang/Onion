using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Core.Contracts.Models
{
    public class ShippingDetails
    {
        [Required(ErrorMessage="Please enter a name")]
        public string Name { get; set; }

        [Required(ErrorMessage="Please enter the first address line")]
        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        public bool GiftWrap { get; set; }
    }


}
