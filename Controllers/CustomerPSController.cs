using BLLProject.Interfaces;
using BLLProject.Repositories;
using BLLProject.Specifications;
using DALProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PLProj.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerPSController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public CustomerPSController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var PS = _unitOfWork.Repository<PartService>().GetAll();
            return View(PS);

            
        }

        public IActionResult Details(int Id)
        {




            ShoppingCart shoppingCart = new ShoppingCart()
            {

                PartService = _unitOfWork.Repository<PartService>().GetFirstOrDefault(e => e.Id == Id, Includes: "ProdServCategory"),
                PartServiceId = Id,
                count = 1
            };
            return View(shoppingCart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            var _user = await _userManager.GetUserAsync(User);
            var spec = new BaseSpecification<Customer>(c => c.AppUserId == _user.Id);
            var customer = _unitOfWork.Repository<Customer>().GetEntityWithSpec(spec);

            if (customer != null)
            {
                shoppingCart.CustomerId = customer.Id;

                // البحث عن الكارت بناءً على CustomerId و PartServiceId
                var cartSpec = new BaseSpecification<ShoppingCart>(u =>
                    u.CustomerId == customer.Id &&
                    u.PartServiceId == shoppingCart.PartServiceId);

                var cartObj = _unitOfWork.Repository<ShoppingCart>().GetEntityWithSpec(cartSpec);

                if (cartObj == null)
                {
                    // إذا كان الكارت غير موجود، نضيفه
                    _unitOfWork.Repository<ShoppingCart>().Add(shoppingCart);
                }
                else
                {
                    // إذا كان الكارت موجودًا، نقوم بزيادة العدد
                    _unitOfWork.shoppingCart.IncreaseCount(cartObj, shoppingCart.count);
                }

                _unitOfWork.Complete();

            }

                return RedirectToAction("Index");
        }
    }
}
