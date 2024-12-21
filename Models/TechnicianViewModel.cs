using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DALProject.Models
{
    public class TechnicianViewModel 
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Technician Name")]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        

        
        public string City { get; set; }
        public string Street { get; set; }


        [Display(Name = "Contact Number")]
        public long ContactNumber { get; set; }
        public virtual Category Category { get; set; } = null!;
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string AppUserId { get; set; }
        public string Availability { get; set; }
        public DateTime BirthDate { get; set; } // .net 5 not support date only

		

        #region Mapping
        public static explicit operator TechnicianViewModel(Technician Model)
        {
            return new TechnicianViewModel
            {
                Id = Model.Id,
                CategoryId = Model.CategoryId,
                
                //Email = Model.Email,
                //City = Model.City,
                //Street = Model.Street,
                //ContactNumber = Model.ContactNumber,
                Availability = Model.Availability,
                BirthDate = Model.BirthDate,
                Category = Model.Category,
                AppUserId = Model.AppUserId,
            };

        }
        public static explicit operator Technician(TechnicianViewModel ViewModel)
        {
            return new Technician
            {
                Id = ViewModel.Id,
                CategoryId = ViewModel.CategoryId,
                //Name = ViewModel.Name,
                //Email = ViewModel.Email,
                //City = ViewModel.City,
                //Street = ViewModel.Street,
                //ContactNumber = ViewModel.ContactNumber,
                Availability = ViewModel.Availability,
                BirthDate = ViewModel.BirthDate,
                AppUserId= ViewModel.AppUserId,
            };

        }
        #endregion
        public static explicit operator AppUser(TechnicianViewModel ViewModel)
        {
            return new AppUser
            {
                UserName = ViewModel.Email,
                Email = ViewModel.Email,
                Name = ViewModel.Name,
                ContactNumber = ViewModel.ContactNumber,
                City = ViewModel.City,
                Street = ViewModel.Street
            };

        }

        //public static explicit operator TechnicianViewModel(AppUser AppModel)
        //{
        //    return new TechnicianViewModel
        //    {
        //        UserName = ViewModel.Email,
        //        Email = ViewModel.Email,
        //        Name = ViewModel.Name,
        //        ContactNumber = ViewModel.ContactNumber,
        //        City = ViewModel.City,
        //        Street = ViewModel.Street
        //    };

        //}

    }

    public static class TechViewModelConvertor
    {
        public static TechnicianViewModel ToTechnicianViewModel ( this Technician technician, AppUser appUser)
        {
            return new TechnicianViewModel
             {
                Id = technician.Id,
                CategoryId = technician.CategoryId,
                Availability = technician.Availability,
                BirthDate = technician.BirthDate,
                Category = technician.Category,
                AppUserId = technician.AppUserId,

                
                Email = appUser.Email,
                Name = appUser.Name,
                ContactNumber = appUser.ContactNumber,
                City = appUser.City,
                Street = appUser.Street


            };


        }


    }
}
