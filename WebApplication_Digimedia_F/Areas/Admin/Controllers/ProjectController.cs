using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WebApplication_Digimedia_F.Context;
using WebApplication_Digimedia_F.Helpers;
using WebApplication_Digimedia_F.Models;
using WebApplication_Digimedia_F.ViewModels.ProjectViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication_Digimedia_F.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _folderpath;
        public ProjectController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _folderpath = Path.Combine(_environment.WebRootPath, "assets", "images");
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects.Select(x=>new ProjectGetVM
            {
                Id=x.Id,
                Name=x.Name,
                ImagePath=x.ImagePath,
                CategoryName=x.Category.Name
            }).ToListAsync();
            return View(projects);
        }
        public async Task<IActionResult> Create()
        {
            await _SendViewbagwithCategory();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateVM vm)
        {
            await _SendViewbagwithCategory();
            if (!ModelState.IsValid)
                return View(vm);
            var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
            if (!isExistCategory)
            {
                ModelState.AddModelError("CategoryId", "bele Category movcud deyil");
                return View(vm);
            }
            if (!vm.Image.GetSize(2))
            {
                ModelState.AddModelError("Image", "sekil 2 mb dan cox ola bilmez");
                return View(vm);
            }
            if (!vm.Image.GetType("image"))
            {
                ModelState.AddModelError("Image", "yalniz sekil yukleye bilersiniz");
            }

            string uniqueFileName = await vm.Image.FileUpload(_folderpath);

            Project project = new()
            {
                Name = vm.Name,
                ImagePath = uniqueFileName,
                CategoryId = vm.CategoryId

            };
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int id)
        {
            var findId = await _context.Projects.FindAsync(id);
            if (findId is null)
              return  NotFound();
            _context.Projects.Remove(findId);
            await _context.SaveChangesAsync();

            string deleteImagePath = Path.Combine(_folderpath, findId.ImagePath);
            FileHelpers.FileDelete(deleteImagePath);
            return RedirectToAction("Index");



        }
        public async Task<IActionResult> Update(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project is null)
                return NotFound();

            ProjectUpdateVM vm = new()
            {
                Id = project.Id,
                CategoryId = project.CategoryId,
                Name = project.Name,
               
            };

            await _SendViewbagwithCategory();
            return View(vm);
        }
        [HttpPost]

        public async Task<IActionResult> UpdateAsync(ProjectUpdateVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);


            var isExistcategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
            if (!isExistcategory)
            {
                ModelState.AddModelError("CategoryId", "This category is not found");
                return View(vm);
            }
            if (!vm.Image?.GetSize(2) ?? false)
            {
                ModelState.AddModelError("Image", "Image's maximum size must be 2 mb");
                return View(vm);
            }
            if (!vm.Image?.GetType("image") ?? false)
            {
                ModelState.AddModelError("Image", "You can upload file in only image format ");
                return View(vm);
            }

            var existProduct = await _context.Projects.FindAsync(vm.Id);
            if (existProduct is null)
                return BadRequest();



            existProduct.Name = vm.Name;
        
            existProduct.CategoryId = vm.CategoryId;
         

            if (vm.Image is { })
            {
                string newImagePath = await vm.Image.FileUpload(_folderpath);
                string oldImagePath = Path.Combine(_folderpath, existProduct.ImagePath);
                FileHelpers.FileDelete(oldImagePath);
                existProduct.ImagePath = newImagePath;
            }

            _context.Projects.Update(existProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        private async Task _SendViewbagwithCategory()
        {
            var category = await  _context.Categories.Select(c=>new SelectListItem()
        {
            Value = c.Id.ToString(),
            Text = c.Name

            }).ToListAsync();
        ViewBag.Categories = category;
        }
    }
}
