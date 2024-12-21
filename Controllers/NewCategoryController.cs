using BLLProject.Interfaces;
using BLLProject.Repositories;
using DALProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PLProj.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NewCategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public NewCategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var categories = _unitOfWork.Repository<ProdServCategory>().GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProdServCategory category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Repository<ProdServCategory>().Add(category);
                _unitOfWork.Complete();
                TempData["Create"] = "Create Done";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            var category = _unitOfWork.Repository<ProdServCategory>().GetFirstOrDefault(c => c.Id == id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProdServCategory category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Repository<ProdServCategory>().Update(category);
                _unitOfWork.Complete();
                TempData["Update"] = "Update Done";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            var category = _unitOfWork.Repository<ProdServCategory>().GetFirstOrDefault(c => c.Id == id);
            return View(category);
        }
        [HttpPost]
        public IActionResult Delete(ProdServCategory category)
        {
            var cat = _unitOfWork.Repository<ProdServCategory>().GetFirstOrDefault(c => c.Id == category.Id);
            if (cat == null)
            {
                NotFound();
            }
            _unitOfWork.Repository<ProdServCategory>().Delete(cat);
            _unitOfWork.Complete();
            TempData["Delete"] = "Delete Done";
            return RedirectToAction("Index");
        }
    }
}
