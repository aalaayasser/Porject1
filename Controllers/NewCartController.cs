using BLLProject.Interfaces;
using BLLProject.Specifications;
using DALProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PLProj.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PLProj.Controllers
{
    [Authorize(Roles = "Customer")]
    public class NewCartController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public NewCartController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;


        }
        public async Task<IActionResult> Index()
        {
            
            var _user = await _userManager.GetUserAsync(User);

            
            var customerSpec = new BaseSpecification<Customer>(c => c.AppUserId == _user.Id);
            var customer = _unitOfWork.Repository<Customer>().GetEntityWithSpec(customerSpec);

            if (customer == null)
            {
                TempData["error"] = "Customer not found.";
                return RedirectToAction("Error"); 
            }

            
            ShoppingCartVM cartVM = new ShoppingCartVM
            {
                CartList = _unitOfWork.shoppingCart.GetAll(
                    d => d.CustomerId == customer.Id,
                    Includes: "PartService"
                ),
                
            };

          
            foreach (var item in cartVM.CartList)
            {
                cartVM.TotalCarts += (item.count * item.PartService.Price);
            }

            return View(cartVM);
        }
        public IActionResult Plus(int cartid)
        {
            var shoopingcart = _unitOfWork.shoppingCart.GetFirstOrDefault(x => x.ShoppingCartId == cartid);
            _unitOfWork.shoppingCart.IncreaseCount(shoopingcart, 1);
            _unitOfWork.Complete();
            return RedirectToAction("Index");

        }
        public IActionResult Minus(int cartid)
        {
            var shoopingcart = _unitOfWork.shoppingCart.GetFirstOrDefault(x => x.ShoppingCartId == cartid);
            if (shoopingcart.count <= 1)
            {
                _unitOfWork.shoppingCart.Delete(shoopingcart);
                _unitOfWork.Complete();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _unitOfWork.shoppingCart.DecreaseCount(shoopingcart, 1);
            }


            _unitOfWork.Complete();
            return RedirectToAction("Index");

        }
        public IActionResult Delete(int cartid)
        {
            var shoopingcart = _unitOfWork.shoppingCart.GetFirstOrDefault(x => x.ShoppingCartId == cartid);
            _unitOfWork.shoppingCart.Delete(shoopingcart);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

    }
}

