using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DALProject.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        [Display(Name = "Contact Number")]
        public long ContactNumber { get; set; }
        public string AppUserId { get; set; }
		public virtual ICollection<Car> Cars { get; set; } = new HashSet<Car>();


		#region Mapping

		public static explicit operator CustomerViewModel(Customer model)
        {
            return new CustomerViewModel
            {
                Id = model.Id,
                //    Street = model.Street,
                //    ContactNumber = model.ContactNumber,
                //    City = model.City,
                //    Email = model.Email,
                AppUserId = model.AppUserId,
                Cars = model.Cars,

            };
        }

        public static explicit operator Customer(CustomerViewModel ViewModel)
        {
            return new Customer
            {
               Id = ViewModel.Id,

                //Street = ViewModel.Street,
                //ContactNumber = ViewModel.ContactNumber,
                //City = ViewModel.City,
                //Email = ViewModel.Email,
                AppUserId = ViewModel.AppUserId,

            };
        }

        public static explicit operator AppUser(CustomerViewModel ViewModel)
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


        #endregion



    }

    public static class CustomerViewModelConvertor
    {
        public static CustomerViewModel ToCustomerViewModel(this Customer customer, AppUser appUser)
        {
            return new CustomerViewModel
            {
                Id = customer.Id,



                Email = appUser.Email,
                Name = appUser.Name,
                ContactNumber = appUser.ContactNumber,
                City = appUser.City,
                Street = appUser.Street


            };


        }


    }

}
