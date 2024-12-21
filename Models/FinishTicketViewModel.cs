using DALProject.Models;
using System;

namespace PLProj.Models
{
    public class FinishTicketViewModel
    {
        public int Id { get; set; }
        public StateType stateType { get; set; }
        public DateTime? EndDateTime { get; set; }

        public string FinalReport { get; set; }


        public static explicit operator FinishTicketViewModel(Ticket model)
        {
            return new FinishTicketViewModel
            {
                Id = model.Id,
                stateType = model.stateType,
                EndDateTime = model.EndDateTime,
                FinalReport = model.FinalReport,



            };
        }
    }
}

