using BLLProject.Interfaces;
using BLLProject.Repositories;
using BLLProject.Specifications;
using DALProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace PLProj.Controllers
{
	[Authorize(Roles = "Admin")]
	public class BrandController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork unitOfWork;
		private readonly IWebHostEnvironment env;

		public BrandController(UserManager<AppUser> userManager, ILogger<HomeController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment env)
		{
			_userManager = userManager;
			_logger = logger;
			this.unitOfWork = unitOfWork;
			this.env = env;
		}
		public IActionResult Index()
		{
			var Brands = unitOfWork.Repository<Brand>().GetAll()
				.Select(s => (BrandViewModel)s).ToList();
			return View(Brands);
		}

        #region Add
        public IActionResult AddBrand()
		{
			return View();
		}
		[HttpPost]
		public IActionResult AddBrand(BrandViewModel model)
		{

			if (ModelState.IsValid)
			{
				unitOfWork.Repository<Brand>().Add((Brand)model);
				var count = unitOfWork.Complete();
				if (count > 0)
				{
					TempData["Message"] = "Brand has been Added Successfully";
					return RedirectToAction(nameof(Index));
				}

			}

			return View(model);
		}
        #endregion

        #region Delete
        public IActionResult Delete(int? Id)
        {
            if (!Id.HasValue)
                return BadRequest();

            var spec = new BaseSpecification<Brand>
            (e => e.Id == Id.Value);
            var Brand = unitOfWork.Repository<Brand>().GetEntityWithSpec(spec);

            if (Brand is null)
                return NotFound();

            return View((BrandViewModel)Brand);
        }
        [HttpPost]
        public IActionResult Delete(BrandViewModel Brand)
        {
            try
            {

                unitOfWork.Repository<Brand>().Delete((Brand)Brand);
                unitOfWork.Complete();
                TempData["message"] = "Brand Deleted Successfully";
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
                return View(Brand);
            }
        }
        #endregion
    }
}
