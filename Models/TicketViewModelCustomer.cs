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
	public class TicketViewModelCustomer 
	{
		//public int Id { get; set; }

		[Required]
		[Display(Name = "Current Kilometres")]
		public long CurrentKilometres { get; set; }

		[Required]
		public string Location { get; set; }
		public string Feedback { get; set; }
		[Display(Name = "Plate Number Of Car ")]
		public int CarId { get; set; }
		[Display(Name = "Serviced Name")]
		//public int ServiceId { get; set; }

        

		public StateType stateType { get; set; }

        #region Mapping

        public static explicit operator TicketViewModelCustomer(Ticket model)
		{
			return new TicketViewModelCustomer
			{
				CurrentKilometres = model.CurrentKilometres,

				Location = model.Location,

				CarId = model.CarId,
				//ServiceId = model.ServiceId,
				Feedback = model.Feedback,
				stateType = model.stateType
				


			};
		}

		public static explicit operator Ticket(TicketViewModelCustomer viewModel)
		{
			return new Ticket
			{
				CurrentKilometres = viewModel.CurrentKilometres,

				Location = viewModel.Location,


				CarId = viewModel.CarId,
				//ServiceId = viewModel.ServiceId,
				Feedback = viewModel.Feedback,
				stateType =viewModel.stateType

			};
		}

		#endregion
	}

}

