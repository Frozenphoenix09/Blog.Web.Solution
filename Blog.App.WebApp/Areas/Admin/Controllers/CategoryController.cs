using Blog.App.Data.Models;
using Blog.App.Service.Service;
using Blog.App.WebApp.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Blog.App.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;

        public CategoryController(ICategoryService categoryService, IUserService userService)
        {
            _categoryService = categoryService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var userID = HttpContext.Session.GetString("UserID");
            if (userID == null) return RedirectToAction("Login", "User", new { Area = "Admin" });

            var user = _userService.GetByID(int.Parse(userID));
            if (user == null) return NotFound();

            ViewBag.UserName = user.UserName.ToString();

            return View(_categoryService.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CatParentId"] = _categoryService.CatSelectList(_categoryService.GetAll());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            ViewData["CatParentId"] = _categoryService.CatSelectList(_categoryService.GetAll());

            if (ModelState.IsValid)
            {
                if (CategoryExist(model.CatName))
                {
                    ModelState.AddModelError("CatName", "Category name already exist !");
                }
                else
                {
                    _categoryService.CreateCategory(model);

                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = _categoryService.GetById(id);

            if (category == null)
            {
                return View("NotFound");
            }
            ViewData["CatParentId"] = _categoryService.CatSelectList(_categoryService.GetAll());

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("CatId,CatName,CatParentId,ShowOnMenu,ShowOnHome,Alias,LayoutType,LayoutDescription")] Category category, string changeCatName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (changeCatName == "on")
                    {
                        if (CategoryExist(category.CatName))
                        {
                            ModelState.AddModelError("CatName", "Category name already exist ! ");
                        }
                        else
                        {
                            try
                            {
                                category.Alias = Utilities.SEOUrl(category.CatName);
                                _categoryService.Update(category);

                                return RedirectToAction(nameof(Index));
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            _categoryService.Update(category);

                            return RedirectToAction(nameof(Index));
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                ViewData["CatParentId"] = _categoryService.CatSelectList(_categoryService.GetAll());
                return View(category);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = _categoryService.GetById(id);
            _categoryService.Delete(category);
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExist(string catName)
        {
            return _categoryService.IsCategoryExist(catName);
        }
    }
}