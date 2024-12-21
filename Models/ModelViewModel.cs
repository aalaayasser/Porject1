using PLProj.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALProject.Models
{
    public class ModelViewModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; } = null!;
        #region Mapping

        public static explicit operator ModelViewModel(Model model)
        {
            return new ModelViewModel
            {
                Id = model.Id,
                Name = model.Name,
                BrandId = model.BrandId,
                Brand = model.Brand,
            };
        }

        public static explicit operator Model(ModelViewModel viewModel)
        {
            return new Model
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                BrandId = viewModel.BrandId,
            };
        }

        #endregion
    }
}
