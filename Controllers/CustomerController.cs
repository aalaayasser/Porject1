using BLLProject.Interfaces;
using BLLProject.Repositories;
using DALProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PLProj.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CustomerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CustomerController> _logger;
        private readonly IWebHostEnvironment env;

        public CustomerController(
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork,
            ILogger<CustomerController> logger, IWebHostEnvironment env)
        {
            _userManager = userManager;

            _unitOfWork = unitOfWork;
            _logger = logger;
            this.env = env;
        }
        public IActionResult Index()
        {

            var Drivs = _unitOfWork.Repository<Customer>().GetAll().Select(t => t).ToList();
            var CustomerViewModelList = new List<CustomerViewModel>();
            Drivs.ForEach(t => CustomerViewModelList.Add(t.ToCustomerViewModel(_userManager.Users.Where(e => e.Id == t.AppUserId).FirstOrDefault())));
            return View(CustomerViewModelList);





        }




    }


}
