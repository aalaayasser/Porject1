using BLLProject.Interfaces;
using BLLProject.Specifications;
using DALProject.Migrations;
using DALProject.Models;
using DALProject.Models.sss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PLProj.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PLProj.Controllers
{
    public class CartController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment env;

        public CartController(UserManager<AppUser> userManager, ILogger<HomeController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _logger = logger;
            this.unitOfWork = unitOfWork;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            var _user = await _userManager.GetUserAsync(User);
            var spec = new BaseSpecification<Customer>(c => c.AppUserId == _user.Id);
            spec.Includes.Add(c => c.CartItems);
            var customer = unitOfWork.Repository<Customer>().GetEntityWithSpec(spec);
            var cartItemCount = customer.CartItems.Count;
			ViewData["CartItemCount"] = cartItemCount;
			var myCartList = customer.CartItems.Select(s => new CartViewModel
            {

                PartId = s.PartId,
                PartName = s.Part.PartName,
                Quantity = s.Quantity,
                Price = s.Part.Price,
                TotalPrice = s.Quantity * s.Part.Price

            }).ToList();
            return View(myCartList);

        }
        [HttpGet]
        [Authorize]
        public IActionResult AddToCart(int partId)
        {
            // Get part details based on the partId
            var spec = new BaseSpecification<Part>(p => p.Id == partId);
            var part = unitOfWork.Repository<Part>().GetEntityWithSpec(spec);

            if (part == null)
            {
                return NotFound("Part not found.");
            }

            // Map the Part entity to a ViewModel for the cart
            var cartItemViewModel = new CartViewModel
            {
                PartId = part.Id,
                PartName = part.PartName,
                Price = part.Price,
                Quantity = 1 // Default quantity
            };

            return View(cartItemViewModel); // Pass the ViewModel to the View
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(CartViewModel cartItem)
        {
            if (!ModelState.IsValid)
            {
                return View(cartItem); 
            }

            
            var _user = await _userManager.GetUserAsync(User);
            var spec = new BaseSpecification<Customer>(c => c.AppUserId == _user.Id);
            var customer = unitOfWork.Repository<Customer>().GetEntityWithSpec(spec);
            var userId = customer.Id;

            var existingCartItem = unitOfWork.Repository<CartItem>()
                     .GetAll() 
                     .Where(c => c.PartId == cartItem.PartId && c.CustomerId == userId)
                     .FirstOrDefault();


            if (existingCartItem != null)
            {
                
                existingCartItem.Quantity += cartItem.Quantity;
                unitOfWork.Repository<CartItem>().Update(existingCartItem);
            }
            else
            {
                
                var newCartItem = new CartItem
                   {
                    PartId = cartItem.PartId,
                    CustomerId = userId,
                    Quantity = cartItem.Quantity
                   };

                unitOfWork.Repository<CartItem>().Add(newCartItem);
            }

            
            unitOfWork.Complete();

            TempData["Message"] = "Part added to cart successfully!";
            return RedirectToAction("Index"); 
        }



    }
}
