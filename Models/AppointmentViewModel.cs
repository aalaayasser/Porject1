using PLProj.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALProject.Models
{
    public class AddAppointmentViewModel
    {
        


        
        [Display(Name = "Start Date & Time")]
        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public int TechnicianId { get; set; }
        [Required]
        public int DriverId { get; set; }
        [Required]
        public int TicketId { get; set; }
        public Car car { get; set; }
        public Service service { get; set; }

        #region Mapping
        public static explicit operator AddAppointmentViewModel(Ticket model)
        {
            var viewmodel = new AddAppointmentViewModel
            {                               
               TicketId = model.Id,
               car = model.Cars,
               service = model.Service,
                
            };
            viewmodel.car.Model = model.Cars.Model;
            viewmodel.car.Model.Brand = model.Cars.Model.Brand;
            viewmodel.car.Color = model.Cars.Color;
            return viewmodel;
        }
         public static explicit operator AddAppointmentViewModel(Appointment model)
        {
            return new AddAppointmentViewModel
            {
                
                
                StartDateTime = model.StartDateTime,
               
                TechnicianId = model.TechnicianId,
                DriverId = model.DriverId,
                TicketId = model.TicketId,
                
            };
        }

        public static explicit operator Appointment(AddAppointmentViewModel viewModel)
        {
            return new Appointment
            {
                
               
                StartDateTime = viewModel.StartDateTime,
               
                TechnicianId = viewModel.TechnicianId,
                DriverId = viewModel.DriverId ,
                TicketId = viewModel.TicketId
            };
        }
        #endregion

    }
}
