using BLLProject.Interfaces;
using DALProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PLProj.Models;
using System.IO;
using System;
using System.Linq;
using BLLProject.Repositories;
using Microsoft.AspNetCore;

namespace PLProj.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PartServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHost;
       
        public PartServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost)
        {
            _unitOfWork = unitOfWork;
            _webHost = webHost;
          
        }
        public IActionResult Index()
        {
            var products = _unitOfWork.Repository<PartService>().GetAll(Includes: "ProdServCategory");
            return View(products);
        }

        public IActionResult Create()
        {
            PartServiceVM vm = new PartServiceVM()
            {
                partService = new PartService(),
                CategoryList = _unitOfWork.Repository<ProdServCategory>().GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),


                })
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PartServiceVM partServiceVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHost.WebRootPath; // WWWroot
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString(); // anyName.jpg to this image   
                    var upload = Path.Combine(RootPath, @"Images\Products\"); // Full Path
                                                                              //var upload = Path.Combine(RootPath, "Images", "Products"); //or

                    // Ensure directory exists
                    if (!Directory.Exists(upload))
                    {
                        Directory.CreateDirectory(upload);
                    }

                    var ext = Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(fileStream); // put image in this path
                    }

                    partServiceVM.partService.Img = @"Images\Products\" + filename + ext;
                }

                _unitOfWork.Repository<PartService>().Add(partServiceVM.partService);
                _unitOfWork.Complete();
                TempData["Create"] = "Create Done";
                return RedirectToAction("Index");
            }
            partServiceVM.CategoryList = _unitOfWork.Repository<ProdServCategory>().GetAll().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            return View(partServiceVM);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }

            PartServiceVM vm = new PartServiceVM()
            {
                partService = _unitOfWork.Repository<PartService>().GetFirstOrDefault(c => c.Id == id),
                CategoryList = _unitOfWork.Repository<ProdServCategory>().GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),


                })
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PartServiceVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _webHost.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    string uploadPath = Path.Combine(rootPath, @"Images\Products\");

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(productVM.partService.Img))
                    {
                        string oldImgPath = Path.Combine(rootPath, productVM.partService.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }

                    // Save the new image
                    string fileExtension = Path.GetExtension(file.FileName);
                    string newFilePath = Path.Combine(uploadPath, fileName + fileExtension);

                    using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.partService.Img = @"Images\Products\" + fileName + fileExtension;
                }

                // تحديث المنتج في قاعدة البيانات
                _unitOfWork.Repository<PartService>().Update(productVM.partService);
                _unitOfWork.Complete();

                TempData["Update"] = "Update Done";
                return RedirectToAction("Index");
            }

            return View(productVM);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            var Product = _unitOfWork.Repository<PartService>().GetFirstOrDefault(c => c.Id == id);
            return View(Product);
        }

        [HttpPost]

        public IActionResult Delete(PartService partService)
        {
            var prod = _unitOfWork.Repository<PartService>().GetFirstOrDefault(c => c.Id == partService.Id);
            if (prod == null)
            {
                NotFound();
            }
            _unitOfWork.Repository<PartService>().Delete(prod);
            var oldImgPath = Path.Combine(_webHost.WebRootPath, prod.Img.TrimStart('\\', '/').Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (System.IO.File.Exists(oldImgPath))
            {
                System.IO.File.Delete(oldImgPath);
            }
            _unitOfWork.Complete();
            TempData["Delete"] = "Create Done";
            return RedirectToAction("Index");


        }








    }
    
}       
