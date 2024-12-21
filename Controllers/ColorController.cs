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
	public class ColorController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment env;

        public ColorController(UserManager<AppUser> userManager, ILogger<HomeController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _logger = logger;
            this.unitOfWork = unitOfWork;
            this.env = env;
        }
        public IActionResult Index()
        {
            var Colors = unitOfWork.Repository<Color>().GetAll()
             .Select(s => (ColorViewModel)s).ToList();
            return View(Colors);
        }

        #region Add
        public IActionResult AddColor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddColor(ColorViewModel model)
        {

            if (ModelState.IsValid)
            {
                unitOfWork.Repository<Color>().Add((Color)model);
                var count = unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Color has been Added Successfully";
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

            var spec = new BaseSpecification<Color>
            (e => e.Id == Id.Value);
            var Brand = unitOfWork.Repository<Color>().GetEntityWithSpec(spec);

            if (Brand is null)
                return NotFound();

            return View((ColorViewModel)Brand);
        }
        [HttpPost]
        public IActionResult Delete(ColorViewModel Brand)
        {
            try
            {

                unitOfWork.Repository<Color>().Delete((Color)Brand);
                unitOfWork.Complete();
                TempData["message"] = "Color Deleted Successfully";
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
