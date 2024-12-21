using BLLProject.Interfaces;
using BLLProject.Specifications;
using DALProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using BLLProject.Repositories;

namespace PLProj.Controllers
{
	public class PartsController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork unitOfWork;
		private readonly IWebHostEnvironment env;

		public PartsController(UserManager<AppUser> userManager, ILogger<HomeController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment env)
		{
			_userManager = userManager;
			_logger = logger;
			this.unitOfWork = unitOfWork;
			this.env = env;
		}
        #region Admin
        [Authorize(Roles = "Admin")]
        
        public IActionResult Index()
		{
			var Parts = unitOfWork.Repository<Part>().GetAll().Select(p => (PartViewModel)p).ToList();
			return View(Parts);
		}



        [Authorize(Roles = "Admin")]
        #region Create
        public IActionResult Create()
		{
			//ViewData["Models"] = unitOfWork.Repository<Model>().GetAll();

			return View();
		}
		[HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(PartViewModel par)
		{
			if (ModelState.IsValid)
			{
				unitOfWork.Repository<Part>().Add((Part)par);
				var count = unitOfWork.Complete();
				if (count > 0)
				{
					TempData["Message"] = "Part has been Added Successfully";
					return RedirectToAction(nameof(Index));
				}

			}

			return View(par);
		}

        #endregion
        [Authorize(Roles = "Admin")]
        #region Details

        public IActionResult Details(int? Id, string viewName = "Details")
		{
			if (!Id.HasValue)
				return BadRequest();

			var spec = new BaseSpecification<Part>(e => e.Id == Id.Value);
			
			var part = unitOfWork.Repository<Part>().GetEntityWithSpec(spec);

			if (part is null)
				return NotFound();

			return View(viewName, (PartViewModel)part);
		}

        #endregion
        [Authorize(Roles = "Admin")]
        #region Edit

        public IActionResult Edit(int? Id)
		{
			return Details(Id, nameof(Edit));
		}

		[HttpPost]

         [Authorize(Roles = "Admin")]
        public IActionResult Edit(PartViewModel par)
		{
			if (!ModelState.IsValid)
				return View(par);

			try
			{
				unitOfWork.Repository<Part>().Update((Part)par);
				unitOfWork.Complete();
				TempData["message"] = "Part Updated Successfully";
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
				return View(par);
			}
		}

        #endregion

        #region Delete
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? Id)
		{
			return Details(Id, nameof(Delete));
		}

		[HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(PartViewModel par)
		{
			try
			{

				unitOfWork.Repository<Part>().Delete((Part)par);
				unitOfWork.Complete();
				TempData["message"] = "Part Deleted Successfully";
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
				return View(par);
			}
		}
        #endregion
        #endregion

        #region Customer

        public async Task<IActionResult> Parts()
        {
            var _user = await _userManager.GetUserAsync(User);
            var spec = new BaseSpecification<Customer>(c => c.AppUserId == _user.Id);           
            var customer = unitOfWork.Repository<Customer>().GetEntityWithSpec(spec);
            var parts = unitOfWork.Repository<Part>().GetAll()
                    .Select(p => new PartViewModel
                    {
                        Id = p.Id,
                        PartName = p.PartName,
                        Price = p.Price,
                        Description = p.Description
                    }).ToList();

            return View(parts);
        }

   
	

        #endregion

    }


}

