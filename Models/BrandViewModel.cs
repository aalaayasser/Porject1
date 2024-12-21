using PLProj.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALProject.Models
{
    public class BrandViewModel 
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }

        #region Mapping
        public static explicit operator BrandViewModel(Brand model)
        {
            return new BrandViewModel
            {
                Id = model.Id,
                BrandName = model.Name,
            };
        }

        public static explicit operator Brand(BrandViewModel viewModel)
        {
            return new Brand
            {
                Id = viewModel.Id,
                Name = viewModel.BrandName,
            };
        }
        #endregion
    }
}




