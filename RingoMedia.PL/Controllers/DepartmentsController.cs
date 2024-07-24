using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using RingoMedia.BLL.Interfaces;
using RingoMedia.DAL.Models;
using RingoMedia.PL.ViewModels;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace RingoMedia.PL.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DepartmentsController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.Departments.GetAllDepartmentsAsync();
            var viewModel = _mapper.Map<IEnumerable<SubDepartmentViewModel>>(departments);
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var department = await _unitOfWork.Departments.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<DepartmentDetailsViewModel>(department);
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.Departments.GetAllDepartmentsAsync();
            ViewBag.Departments = _mapper.Map<IEnumerable<SubDepartmentViewModel>>(departments);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Logo != null)
                {
                    uniqueFileName = await SaveLogoAsync(model.Logo);
                }

                var department = _mapper.Map<Department>(model);
                department.LogoPath = uniqueFileName;
                await _unitOfWork.Departments.AddDepartmentAsync(department);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }

            var departments = await _unitOfWork.Departments.GetAllDepartmentsAsync();
            ViewBag.Departments = _mapper.Map<IEnumerable<SubDepartmentViewModel>>(departments);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var department = await _unitOfWork.Departments.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<DepartmentEditViewModel>(department);

            // Fetch the list of departments for the ParentDepartmentId dropdown
            var departments = await _unitOfWork.Departments.GetAllDepartmentsAsync();
            viewModel.Departments = new SelectList(departments, "Id", "Name", viewModel.ParentDepartmentId);

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var department = await _unitOfWork.Departments.GetDepartmentByIdAsync(id);
                if (department == null)
                {
                    return NotFound();
                }

                if (model.Logo != null)
                {
                    // Check if there is an existing logo and delete it if it exists
                    if (!string.IsNullOrEmpty(department.LogoPath))
                    {
                        var oldLogoFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", Path.GetFileName(department.LogoPath));
                        if (System.IO.File.Exists(oldLogoFilePath))
                        {
                            System.IO.File.Delete(oldLogoFilePath);
                        }
                    }

                    // Save the new logo and update the logo path
                    var newLogoPath = await SaveLogoAsync(model.Logo);
                    department.LogoPath = newLogoPath;
                }

                _mapper.Map(model, department);
                _unitOfWork.Departments.UpdateDepartmentAsync(department);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction(nameof(Index));
            }

            // Re-populate the dropdown list in case of validation failure
            var departments = await _unitOfWork.Departments.GetAllDepartmentsAsync();
            model.Departments = new SelectList(departments, "Id", "Name", model.ParentDepartmentId);

            return View(model);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var department = await _unitOfWork.Departments.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<DepartmentDetailsViewModel>(department);
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _unitOfWork.Departments.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            // Delete the logo file if it exists
            if (!string.IsNullOrEmpty(department.LogoPath))
            {
                var logoFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", Path.GetFileName(department.LogoPath));
                if (System.IO.File.Exists(logoFilePath))
                {
                    System.IO.File.Delete(logoFilePath);
                }
            }

            await _unitOfWork.Departments.DeleteDepartmentAsync(id);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveLogoAsync(IFormFile logo)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + logo.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await logo.CopyToAsync(fileStream);
            }

            return "/images/" + uniqueFileName;
        }
    }
}
