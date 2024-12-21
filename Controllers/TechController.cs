using BLLProject.Interfaces;
using BLLProject.Repositories;
using BLLProject.Specifications;
using DALProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLProj.Controllers
{
	[Authorize(Roles = "Admin")]
	public class TechController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TechController> _logger;
        private readonly IWebHostEnvironment env;

        public TechController(
            UserManager<AppUser> userManager,
          
            IUnitOfWork unitOfWork,
            ILogger<TechController> logger, IWebHostEnvironment env)
        {
            _userManager = userManager;
          
            _unitOfWork = unitOfWork;
            _logger = logger;
            this.env = env;
        }

        public IActionResult Index()
        {
            var spec = new BaseSpecification<Technician>();
            spec.Includes.Add(e => e.Category);
            var techs = _unitOfWork.Repository<Technician>().GetAllWithSpec(spec).Select(t => t).ToList();
            var techViewModelList = new List<TechnicianViewModel>();
            techs.ForEach(t => techViewModelList.Add(t.ToTechnicianViewModel(_userManager.Users.Where(e => e.Id == t.AppUserId).FirstOrDefault())));
            return View(techViewModelList);


        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(TechnicianViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = (AppUser)model;
                    /*new AppUser { *//*UserName = model.Email, Email = model.Email*//*, Name = model.Name, ContactNumber = model.ContactNumber ,City = model.City, Street = model.Street*/
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Technician");
                    model.AppUserId = user.Id;
                    //var technician = new TechnicianViewModel
                    //{
                    //    AppUserId = user.Id,

                    //    //Availability = model.Availability,
                    //    //BirthDate = model.BirthDate,
                    //    //CategoryId = model.CategoryId,    
                    //    //Name = model.Name,
                    //    //ContactNumber = model.ContactNumber,
                    //    //Email = model.Email,
                    //    //City = model.City,
                    //    //Street = model.Street,






                    //};

                    _unitOfWork.Repository<Technician>().Add((Technician)model);
                    _unitOfWork.Complete();

                    _logger.LogInformation("Technician created a new account with password.");

                    return RedirectToAction("Index");

                }


                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public IActionResult Details(int? Id, string viewname = "Details")
        {
            if (!Id.HasValue)
                return BadRequest();
            
            var spec = new BaseSpecification<Technician>(e => e.Id == Id.Value);
            spec.Includes.Add(e => e.Category);
            var Tech = _unitOfWork.Repository<Technician>().GetEntityWithSpec(spec);
            if (Tech is null)
                return NotFound();
            var appUser = _userManager.Users.Where( e => e.Id == Tech.AppUserId).FirstOrDefault();

            return View(viewname, Tech.ToTechnicianViewModel(appUser));
        }

      
        public IActionResult Edit(int? Id)
        {


            return Details(Id, nameof(Edit));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int Id, TechnicianViewModel Tech)
        {
            if (Id != Tech.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(Tech);

            try
            {

                _unitOfWork.Repository<Technician>().Update((Technician)Tech);
                _unitOfWork.Complete();
                TempData["Message"] = "Technician Has been Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                if (env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the Department");

                return View(Tech);
            }
        }
        public IActionResult Delete(int? Id)
        {

            return Details(Id, nameof(Delete));

        }

        [HttpPost]
        public IActionResult Delete(TechnicianViewModel Tech)
        {
            try
            {
                _unitOfWork.Repository<Technician>().Delete((Technician)Tech);
                _unitOfWork.Complete();
                TempData["Message"] = "Technicain Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                
                if (env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Deleting the Department");

                return View(Tech);
            }
        }
    }




}  
    

	


