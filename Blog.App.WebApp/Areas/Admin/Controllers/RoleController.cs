using Blog.App.Data.Models;
using Blog.App.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Blog.App.WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public RoleController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var userID = HttpContext.Session.GetString("UserID");
            if (userID == null) return RedirectToAction("Login", "User", new { Area = "Admin" });

            var user = _userService.GetByID(int.Parse(userID));
            if (user == null) return NotFound();

            return View(_roleService.GetAll());
        }

        public async Task<IActionResult> Details(int id)
        {
            var role = _roleService.GetById(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role role)
        {
            if (ModelState.IsValid)
            {
                if (RoleAlreadyExist(role.RoleName))
                {
                    ModelState.AddModelError("RoleName", "Role Name already exist !");
                }
                else
                {
                    _roleService.CreateRole(role);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(role);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var role = _roleService.GetById(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Role role, string changeRoleName)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (changeRoleName == "on")
                    {
                        if (RoleAlreadyExist(role.RoleName))
                        {
                            ModelState.AddModelError("RoleName", "Role name already exist !");
                        }
                        else
                        {
                            _roleService.Update(role);
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        _roleService.Update(role);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View(role);
        }

        // GET: admin/Roles/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var role = _roleService.GetById(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: admin/Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = _roleService.GetById(id);
            _roleService.Delete(role);

            return RedirectToAction(nameof(Index));
        }

        private bool RoleAlreadyExist(string roleName)
        {
            return _roleService.IsRoleAlreadyExist(roleName);
        }
    }
}