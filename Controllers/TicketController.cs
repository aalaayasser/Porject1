using BLLProject.Interfaces;
using BLLProject.Specifications;
using DALProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PLProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace PLProj.Controllers
{
    public class TicketController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment env;

        public TicketController(UserManager<AppUser> userManager, ILogger<HomeController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _logger = logger;
            this.unitOfWork = unitOfWork;
            this.env = env;
        }

		#region User
		
		
		[Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyTicket()
        {
            var _user = await _userManager.GetUserAsync(User);
            var spec = new BaseSpecification<Customer>(c => c.AppUserId == _user.Id);
            spec.Includes.Add(t => t.Cars);
            //spec.AllIncludes.Add(t => t.Include(c => c.Cars).ThenInclude(c => c.Tickets));
            var customer = unitOfWork.Repository<Customer>().GetEntityWithSpec(spec);

            var myTicketList = new List<Ticket>();

            foreach (var item in customer?.Cars)
            {
                foreach (var tic in item.Tickets)
                {
                    myTicketList.Add(tic);
                }
            }

            //var myTicketList = customer?.Cars?.SelectMany(s => s.Tickets.SelectMany(t=>t)).ToList();

            //ViewData["Services"] = unitOfWork.Repository<Service>().GetAll();

            return View(myTicketList);
        }
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddTicket()
        {

            var _user = await _userManager.GetUserAsync(User);
            var spec = new BaseSpecification<Customer>(c => c.AppUserId == _user.Id);
            spec.Includes.Add(t => t.Cars);
            var customer = unitOfWork.Repository<Customer>().GetEntityWithSpec(spec);
            var myCarList = customer.Cars;
            ViewData["CarList"] = myCarList;
            ViewData["Services"] = unitOfWork.Repository<Service>().GetAll();
            

            return View();
        }
        [HttpPost]
		[Authorize(Roles = "Customer")]
		public async Task<IActionResult> AddTicket(TicketViewModelCustomer ticket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ticket.stateType = StateType.New;
                    unitOfWork.Repository<Ticket>().Add((Ticket)ticket);
                    unitOfWork.Complete();
                    TempData["Message"] = "Ticket has been Added Successfully";

                    return RedirectToAction(nameof(MyTicket));
                }
                catch (Exception ex)
                {
                    if (env.IsDevelopment())
                        ModelState.AddModelError(string.Empty, ex.Message);
                    else
                        ModelState.AddModelError(string.Empty, "An Error Has Occurred during Deleting the Employee");
                }
            }
            return View(ticket);
        }
        #endregion

        #region Admin => to manage opreations
        [Authorize(Roles = "Admin")]
        public IActionResult AllTicket()
        {
            var spec = new BaseSpecification<Ticket>(e => e.stateType == StateType.New);
            var ticketList = unitOfWork.Repository<Ticket>().GetAllWithSpec(spec);


            return View(ticketList);
        }
		[Authorize(Roles = "Admin")]
		public IActionResult AddAppointment(int? Id)
        {
            if (!Id.HasValue)
                return BadRequest();

            var spec = new BaseSpecification<Ticket>(e => e.Id == Id.Value);

            var ticket = unitOfWork.Repository<Ticket>().GetEntityWithSpec(spec);

            if (ticket is null)
                return NotFound();
            ViewData["Technicain"] = new SelectList(unitOfWork.Repository<Technician>().GetAll().Select(e => new { Id = e.Id, Name = e.user.Name }), "Id", "Name");
            ViewData["Driver"] = new SelectList(unitOfWork.Repository<Driver>().GetAll().Select(e => new { Id = e.Id, Name = e.user.Name }), "Id", "Name");


            return View((AddAppointmentViewModel)ticket);

        }
        [HttpPost]
		[Authorize(Roles = "Admin")]
		public IActionResult AddAppointment([FromRoute] int? Id, AddAppointmentViewModel model)
        {
            if (!Id.HasValue)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    model.TicketId = Id.Value;
                    unitOfWork.Repository<Appointment>().Add((Appointment)model);
                    var spec = new BaseSpecification<Ticket>(e => e.Id == Id);
                    var ticket = unitOfWork.Repository<Ticket>().GetEntityWithSpec(spec);
                    if (ticket is not null)
                    {
                        ticket.StartDateTime = model.StartDateTime;
                        ticket.stateType = StateType.Assigned;

                    }
                    unitOfWork.Repository<Ticket>().Update(ticket);

                    unitOfWork.Complete();


                    //TempData["Message"] = "Appointment has been Added Successfully";

                    return RedirectToAction(nameof(AllTicket));
                }
                catch (Exception ex)
                {
                    if (env.IsDevelopment())
                        ModelState.AddModelError(string.Empty, ex.Message);
                    else
                        ModelState.AddModelError(string.Empty, "An Error Has Occurred during At Appointment ");

                }
            }
            return View(model);

        }

        #endregion

        #region Technician
        [Authorize(Roles = "Technician")]

        public async Task<IActionResult> AssignedTechnicainTicket()
        {

            var user = await _userManager.GetUserAsync(User);
            var techspec = new BaseSpecification<Technician>(e => e.AppUserId == user.Id);
            var tech = unitOfWork.Repository<Technician>().GetEntityWithSpec(techspec);
            var spec = new BaseSpecification<Ticket>(e => e.stateType == StateType.Assigned && e.Appointments.Any(ap => ap.TechnicianId == tech.Id));
            var ticketList = unitOfWork.Repository<Ticket>().GetAllWithSpec(spec);
            return View(ticketList);
        }

        [Authorize(Roles = "Technician")]

        public async Task<IActionResult> FinishTicket(int id)
        {
            var ticketSpec = new BaseSpecification<Ticket>(e => e.Id == id);
            var ticket = unitOfWork.Repository<Ticket>().GetEntityWithSpec(ticketSpec);

            if (ticket == null)
            {
                return NotFound();
            }

            var viewModel = new FinishTicketViewModel
            {
                Id = ticket.Id,
                stateType = ticket.stateType 
            };

            return View(viewModel);
        }


        [HttpPost]
        [Authorize(Roles = "Technician")]
        public async Task<IActionResult> FinishTicket(FinishTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ticketSpec = new BaseSpecification<Ticket>(e => e.Id == model.Id);
                var ticket = unitOfWork.Repository<Ticket>().GetEntityWithSpec(ticketSpec);

                if (ticket != null)
                {
                    ticket.EndDateTime = model.EndDateTime; 
                    ticket.stateType = StateType.Finished;

                    unitOfWork.Repository<Ticket>().Update(ticket);
                    unitOfWork.Complete();

                    TempData["Message"] = "Ticket has been finished successfully";
                    return RedirectToAction(nameof(AssignedTechnicainTicket));
                }

                return NotFound();
            }

            return View(model); 
        }



        #endregion

        #region Driver
        [Authorize(Roles = "Driver")]

        public async Task<IActionResult> AssignedDriverTicket()
        {

            var user = await _userManager.GetUserAsync(User);
            var Driverspec = new BaseSpecification<Driver>(e => e.AppUserId == user.Id);
            var driver = unitOfWork.Repository<Driver>().GetEntityWithSpec(Driverspec);
            var spec = new BaseSpecification<Ticket>(e => e.stateType == StateType.Assigned && e.Appointments.Any(ap => ap.DriverId == driver.Id));
            var ticketList = unitOfWork.Repository<Ticket>().GetAllWithSpec(spec);
            return View(ticketList);
        }


        #endregion


    }
}
