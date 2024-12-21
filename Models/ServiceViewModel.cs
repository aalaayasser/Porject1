using PLProj.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALProject.Models
{
    public class ServiceViewModel 
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Required]
        public decimal Price{ get; set; }
        //public DateTime EstimatedTime { get; set; }
        public string Description{ get; set; }

        public virtual Category Category { get; set; } = null!;
        #region Mapping
        public static explicit operator ServiceViewModel(Service model)
        {
            return new ServiceViewModel
            {
                Id = model.Id,
                CategoryId = model.CategoryId,
                Name = model.Name,
                Price = model.Price,
                //EstimatedTime = model.EstimatedTime,
                Description = model.Description,
                Category = model.Category
            };
        }

        public static explicit operator Service(ServiceViewModel viewModel)
        {
            return new Service
            {
                Id = viewModel.Id,
                CategoryId = viewModel.CategoryId,
                Name = viewModel.Name,
                Price = viewModel.Price,
                //EstimatedTime = viewModel.EstimatedTime,
                Description = viewModel.Description,
                
            };
        }
        #endregion
    }
}







