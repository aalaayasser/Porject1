using PLProj.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALProject.Models
{
	public class CarViewModel
	{
        
        public int Id { get; set; }
		[Required]
		[Display(Name = "Plate Number")]
        public string PlateNumber { get; set; }

        [Display(Name = "Model")]
		public int ModelId { get; set; }

		[Display(Name = "Customer")]
		public int CustomerId { get; set; }

		[Display(Name = "Color")]
		public int ColorId { get; set; }
		[Required]

		[Display(Name = "Year of Manufacture")]
		public int Year { get; set; }

		[Display(Name = "Description")]
		public string? Description { get; set; }
        public virtual Model Model { get; set; } = null!;
        public virtual Color Color { get; set; } = null!;


		#region Mapping

		public static explicit operator CarViewModel(Car model)
		{
			var viewmodel = new CarViewModel()
			{
				Id = model.Id,
				ModelId = model.ModelId,
				CustomerId = model.CustomerId,
				ColorId = model.ColorId,
				Year = model.Year,
				Description = model.Description,
				Model = model.Model,
				Color = model.Color,
				PlateNumber = model.PlateNumber,

			};
			viewmodel.Model.Brand = model.Model.Brand;
			return viewmodel;
        }

        public static explicit operator Car(CarViewModel viewModel)
		{
			return new Car
			{
				Id = viewModel.Id,
				ModelId = viewModel.ModelId,
				CustomerId = viewModel.CustomerId,
				ColorId = viewModel.ColorId,
				Year = viewModel.Year,
				Description = viewModel.Description,
				PlateNumber= viewModel.PlateNumber,
			};
		}

		#endregion

	}
}
