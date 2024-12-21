
using DALProject.Models;
using System.Collections.Generic;

namespace PLProj.Models
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart>? CartList { get; set; }
                public decimal TotalCarts { get; set; }


    }
}
