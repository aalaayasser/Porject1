using PLProj.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALProject.Models
{
    public class PartViewModel 
    {
       
        public int Id { get; set; }
        [Required]
        [Display(Name = "Part Name")]
        public string PartName { get; set; }

        [Required(ErrorMessage = "Part Price is required.")]
        
        public int Price { get; set; }

        [Required(ErrorMessage = "Part Kilometres to Change is required.")]
        [Display(Name = "Kilometres to Change Part")]
        public long PartKilometresToChange { get; set; }

        
        public string Description { get; set; }


        #region Mapping
        public static explicit operator PartViewModel(Part model)
        {
            return new PartViewModel
            {
                Id = model.Id,
                PartName = model.PartName,
                Price = model.Price,
                PartKilometresToChange = model.PartKilometresToChange,
                
               Description = model.Description

            };
        }

        public static explicit operator Part(PartViewModel viewModel)
        {
            return new Part
            {
                Id = viewModel.Id,
                PartName = viewModel.PartName,
                Price = viewModel.Price,
                PartKilometresToChange = viewModel.PartKilometresToChange,
                
                Description = viewModel.Description
                
                
            };
        }
        #endregion
    }
}