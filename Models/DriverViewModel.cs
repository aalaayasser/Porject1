using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DALProject.Models
{
    public class  DriverViewModel 
    {

        public int Id { get; set; }
        [Required]
        [Display(Name = "Driver Name")]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        [Display(Name = "Contact Number")]
        public long ContactNumber { get; set; }

        public string Availability { get; set; }

        public DateTime BirthDate { get; set; } // .net 5 not support date only
        

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string AppUserId { get; set; }

        [Required]
        public string License  { get; set; }

        [Display(Name = "License Date")]
        [Required]
        public DateTime LicenseDate { get; set; }

        [Display(Name = "License Expiry Date")]
        public DateTime LicenseExpDate { get; set; }




        #region Mapping

        public static explicit operator DriverViewModel(Driver model)
        {
            return new DriverViewModel
            {
                Id = model.Id,
                //Name = model.Name,
                //Street = model.Street,
                //ContactNumber = model.ContactNumber,
                //City = model.City,
                //Email = model.Email,
                Availability = model.Availability,
                BirthDate = model.BirthDate,
                License = model.License,
                LicenseDate = model.LicenseDate,
                LicenseExpDate = model.LicenseExpDate,
                AppUserId = model.AppUserId,
                
                
            };
        }

        public static explicit operator Driver(DriverViewModel ViewModel)
        {
            return new Driver
            {
                Id = ViewModel.Id,
                //Name = ViewModel.Name,
                //Street = ViewModel.Street,
                //ContactNumber = ViewModel.ContactNumber,
                //City = ViewModel.City,
                //Email = ViewModel.Email,
                Availability = ViewModel.Availability,
                BirthDate = ViewModel.BirthDate,
                License = ViewModel.License,
                LicenseDate = ViewModel.LicenseDate,
                LicenseExpDate = ViewModel.LicenseExpDate,
                    AppUserId = ViewModel.AppUserId,
                
            };
        }

        #endregion
        public static explicit operator AppUser(DriverViewModel ViewModel)
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

      

    }

    public static class DriverViewModelConvertor
    {
        public static DriverViewModel ToDriverViewModel(this Driver driver, AppUser appUser)
        {
            return new DriverViewModel
            {
                Id = driver.Id,
                Availability = driver.Availability,
                BirthDate = driver.BirthDate,
                AppUserId = driver.AppUserId,
                License = driver.License,
                LicenseDate = driver.LicenseDate,
                LicenseExpDate = driver.LicenseExpDate,


                Email = appUser.Email,
                Name = appUser.Name,
                ContactNumber = appUser.ContactNumber,
                City = appUser.City,
                Street = appUser.Street


            };


        }


    }
}
