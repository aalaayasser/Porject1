using BLLProject.Interfaces;
using BLLProject.Specifications;
using DALProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace PLProj.Controllers
{
	[Authorize(Roles ="Admin")]
	

    public class ServicesController : Controller
    {
        private readonly ILogger<ServicesController> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment env;

        public ServicesController(ILogger<ServicesController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.env = env;
        }

        #region Get
        public IActionResult Index()
        {
            var Services = unitOfWork.Repository<Service>().GetAll().Select(s => (ServiceViewModel)s).ToList();
            return View(Services);
        }

		#endregion

		#region CreateServic
		public IActionResult Create()
        {
            

            return View();
        }
        [HttpPost]
        public IActionResult Create(ServiceViewModel serv)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Repository<Service>().Add((Service)serv);
                var count = unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Service has been Added Successfully";
                    return RedirectToAction(nameof(Index));
                }

            }

            return View(serv);
        }

		#endregion

		#region Details

		public IActionResult Details(int? Id , string viewName = "Details")
		{
			if (!Id.HasValue)
				return BadRequest();
									
			var spec = new BaseSpecification<Service>
			(e => e.Id == Id.Value);
			spec.Includes.Add(e => e.Category);
			var service = unitOfWork.Repository<Service>().GetEntityWithSpec(spec);

			if (service is null)
				return NotFound(); 
								   
			return View(viewName, (ServiceViewModel) service);
		}

		#endregion

		#region Edit

		public IActionResult Edit(int? Id)
		{
			return Details(Id, nameof(Edit));
		}

        [HttpPost]
        public IActionResult Edit(ServiceViewModel emp)
		{
			if (!ModelState.IsValid)
				return View(emp);

			try
			{
				unitOfWork.Repository<Service>().Update((Service)emp);
				unitOfWork.Complete();
				TempData["message"] = "Service Updated Successfully";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				if (env.IsDevelopment())
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
				else
				{
					ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the Employee");
				}
				return View(emp);
			}
		}

		#endregion

		#region Delete
		public IActionResult Delete(int? Id)
		{
			return Details(Id, nameof(Delete));
		}

		[HttpPost]
		public IActionResult Delete(ServiceViewModel sev)
		{
			try
			{

				unitOfWork.Repository<Service>().Delete((Service)sev);
				unitOfWork.Complete();
				TempData["message"] = "Service Deleted Successfully";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{

				if (env.IsDevelopment())
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
				else
				{
					ModelState.AddModelError(string.Empty, "An Error Has Occurred during Deleted the Service");
				}
				return View(sev);
			}
		}
		#endregion

	}
}
