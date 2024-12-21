using BLLProject.Interfaces;
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
	public class ModelController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork unitOfWork;
		private readonly IWebHostEnvironment env;

		public ModelController(UserManager<AppUser> userManager, ILogger<HomeController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment env)
		{
			_userManager = userManager;
			_logger = logger;
			this.unitOfWork = unitOfWork;
			this.env = env;
		}

        public IActionResult Index()
        {
            var Model = unitOfWork.Repository<Model>().GetAll().Select(s => (ModelViewModel)s).ToList();
            return View(Model);
		}

        #region Add

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ModelViewModel model)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Repository<Model>().Add((Model)model);
                var count = unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Model has been Added Successfully";
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

            var spec = new BaseSpecification<Model>
            (e => e.Id == Id.Value);
            var Brand = unitOfWork.Repository<Model>().GetEntityWithSpec(spec);

            if (Brand is null)
                return NotFound();

            return View((ModelViewModel)Brand);
        }
        [HttpPost]
        public IActionResult Delete(ModelViewModel model)
        {
            try
            {

                unitOfWork.Repository<Model>().Delete((Model)model);
                unitOfWork.Complete();
                TempData["message"] = "Model Deleted Successfully";
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
                return View(model);
            }
        }
        #endregion

    }
}
