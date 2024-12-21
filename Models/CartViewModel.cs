using DALProject.Models;
using DALProject.Models.sss;

namespace PLProj.Models
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public string PartName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int TotalPrice { get; set; }

        public virtual Part Part { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }


        public static explicit operator CartViewModel(CartItem model)
        {
            return new CartViewModel
            {
                Id = model.Id,
               PartId = model.Id,
               PartName = model.Part.PartName,
               Quantity = model.Quantity,
               Price = model.Part.Price,
               CustomerId = model.CustomerId,

               Customer = model.Customer


            };
        }

        public static explicit operator CartItem(CartViewModel ViewModel)
        {
            return new CartItem
            {
                Id = ViewModel.Id,
                PartId = ViewModel.Part.Id,
                
                Quantity = ViewModel.Quantity,
                
                CustomerId = ViewModel.CustomerId,

            };
        }
    }
}
