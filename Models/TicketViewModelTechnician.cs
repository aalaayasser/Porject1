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
    public class TicketViewModelTechnician 
    {
        
        
         
        public int Id { get; set; }
        [Display(Name = "Start Date & Time")]
        public DateTime? StartDateTime { get; set; }

       
        

        [Display(Name = "Final Report")]
        public string FinalReport { get; set; } = null!;

        [Display(Name = "End Date & Time")]
        public DateTime? EndDateTime { get; set; }

        
		

		#region Mapping

		public static explicit operator TicketViewModelTechnician(Ticket model)
        {
            return new TicketViewModelTechnician
			{
                //Id = model.Id,
               
                StartDateTime = model.StartDateTime,
               
                
                FinalReport = model.FinalReport,
                EndDateTime = model.EndDateTime,
               
                //CarId = model.CarId,
                //ServiceId = model.ServiceId,
                
                //IsPayed = model.IsPayed,
                
            };
        }

        public static explicit operator Ticket(TicketViewModelTechnician viewModel)
        {
            return new Ticket
            {
                //Id = viewModel.Id,
                
                StartDateTime = viewModel.StartDateTime,
                
                
                FinalReport = viewModel.FinalReport,
                EndDateTime = viewModel.EndDateTime,
               
                //CarId = viewModel.CarId,
                //ServiceId = viewModel.ServiceId,
                
                //IsPayed = viewModel.IsPayed,
                
            };
        }

        #endregion
    }

}

