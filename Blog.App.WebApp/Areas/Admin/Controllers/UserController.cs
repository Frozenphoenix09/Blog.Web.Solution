using Blog.App.Data.Models;
using Blog.App.Service.Service;
using Blog.App.WebApp.Areas.Admin.Models;
using Blog.App.WebApp.Enums;
using Blog.App.WebApp.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.App.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public IActionResult Index(int roleID = 0)
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var userID = HttpContext.Session.GetString("UserID");
            if (userID == null) return RedirectToAction("Login", "User", new { Area = "Admin" });

            ViewData["RoleId"] = _roleService.RoleSelectList(_roleService.GetAll());

            List<User> result = new List<User>();

            var user = _userService.GetByID(int.Parse(userID));

            result.Add(user);

            if (user == null) return NotFound();

            var users = _userService.GetAllUser();

            if (user.Role != null && user.Role.IsAdmin == true)
            {
                if (roleID == 0)
                {
                    return View(users);
                }
                else if (roleID != 0)
                {
                    return View(_userService.GetByRole(roleID));
                }
            }
            else if (user.Role != null && user.Role.IsAdmin == false)
            {
                return View(result);
            }
            return View(result);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["RoleId"] = _roleService.RoleSelectList(_roleService.GetAll());

            var enumData = from UserStatus e in Enum.GetValues(typeof(UserStatus))
                           select new
                           {
                               Text = e,
                               Value = e
                           };
            ViewData["UserStatus"] = new SelectList(enumData, "Text", "Value");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId,UserName,Email,Phone,Passwd,Thumb")] User user, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (EmailAreadyExist(user.Email))
                    {
                        ModelState.AddModelError("Email", "Email already exist !");
                    }
                    else
                    {
                        if (fThumb != null)
                        {
                            string extension = Path.GetExtension(fThumb.FileName);
                            string Newname = Utilities.SEOUrl(user.UserName) + extension;
                            user.Thumb = await Utilities.UploadFile(fThumb, @"users\", Newname.ToLower());
                        }

                        user.LastLogin = null;
                        user.Salt = Utilities.GetRandomKey();
                        var passwordMd5 = (user.Passwd + user.Salt).ToMD5();
                        user.Passwd = passwordMd5;

                        _userService.Insert(user);

                        return RedirectToAction(nameof(Index));
                    }
                }
                var enumData = from UserStatus e in Enum.GetValues(typeof(UserStatus))
                               select new
                               {
                                   Text = e,
                                   Value = e
                               };
                ViewData["UserStatus"] = new SelectList(enumData, "Text", "Value");

                ViewData["RoleId"] = _roleService.RoleSelectList(_roleService.GetAll());

                return View(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "Login")]
        public IActionResult Login(string returnUrl = null)
        {
            if (TempData["shortMessage"] != null)
            {
                ViewBag.Message = TempData["shortMessage"].ToString();
            }

            var UserID = HttpContext.Session.GetString("UserID");
            if (UserID != null) return RedirectToAction("Index", "Home", new { Area = "Admin" });
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "Login")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = _userService.GetByEmail(model.Email);
                    if (user == null)
                    {
                        ViewBag.Error = "Email or Password incorrect ! ";
                        return View(model);
                    }
                    string passwd = (model.Password.Trim() + user.Salt.Trim()).ToMD5();

                    if (user.Passwd.Trim() != passwd)
                    {
                        ViewBag.Error = "Email or Password incorrect ! ";
                        return View(model);
                    }
                    if (user.Status == "Inactive")
                    {
                        ViewBag.Error = "Access denied ! ";
                        return View(model);
                    }

                    user.LastLogin = DateTime.Now;
                    _userService.Update(user);

                    var userID = HttpContext.Session.GetString("UserID");

                    HttpContext.Session.SetString("UserID", user.UserId.ToString());
                    HttpContext.Session.SetString("UserName", user.UserName);

                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim("UserID",user.UserId.ToString()),
                        new Claim("RoleID",user.RoleId.ToString()),
                        new Claim(ClaimTypes.Role,user.Role.RoleName)
                    };

                    var grandmaIdentity = new ClaimsIdentity(userClaims, "Users Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    await HttpContext.SignInAsync(userPrincipal);

                    GlobalVariable.CurrentUser = user.UserName;
                    GlobalVariable.CurrentUserID = user.UserId;
                    if (user.Role != null && user.Role.IsAdmin)
                    {
                        GlobalVariable.IsAdminUser = true;
                    }
                    else
                    {
                        GlobalVariable.IsAdminUser = false;
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "User", new { Area = "Admin" });
            }
            return RedirectToAction("Login", "User", new { Area = "Admin" });
        }

        public IActionResult Logout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("UserID");

                GlobalVariable.CurrentUser = "";
                GlobalVariable.CurrentUserID = null;
                GlobalVariable.IsAdminUser = false;

                return RedirectToAction("Login", "User", new { Area = "admin" });
            }
            catch
            {
                return RedirectToAction("Login", "User", new { Area = "admin" });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var userID = HttpContext.Session.GetString("UserID");

            if (userID == null) return RedirectToAction("Login", "User", new { Area = "Admin" });

            var user = _userService.GetByID(int.Parse(userID));

            var userDetail = _userService.GetByID(id);

            if (userDetail == null)
            {
                return NotFound();
            }

            if (user.Role != null && user.Role.IsAdmin == true)
            {
                return View(userDetail);
            }
            else if (user.Role != null && user.Role.IsAdmin == false)
            {
                if (id != user.UserId)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(userDetail);
                }
            }
            else if (user.Role == null)
            {
                if (id != user.UserId)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(userDetail);
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

            var userID = HttpContext.Session.GetString("UserID");
            if (userID == null) return RedirectToAction("Login", "User", new { Area = "Admin" });

            var user = _userService.GetByID(int.Parse(userID));

            var userToEdit = _userService.GetByID(id);

            if (user == null) return NotFound();

            var enumData = from UserStatus e in Enum.GetValues(typeof(UserStatus))
                           select new
                           {
                               Text = e,
                               Value = e
                           };
            ViewData["UserStatus"] = new SelectList(enumData, "Text", "Value");
            ViewData["RoleId"] = _roleService.RoleSelectList(_roleService.GetAll());

            if (user.Role != null && user.Role.IsAdmin == true)
            {
                return View(userToEdit);
            }
            else if (user.Role != null && user.Role.IsAdmin == false)
            {
                if (id != user.UserId)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(userToEdit);
                }
            }
            else if (user.Role == null)
            {
                if (id != user.UserId)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(userToEdit);
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UserId,RoleId,UserName,Email,Phone,Passwd,Salt,Status,Thumb,LastLogin")] User user, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string Newname = Utilities.SEOUrl(user.UserName) + extension;
                        user.Thumb = await Utilities.UploadFile(fThumb, @"users\", Newname.ToLower());
                    }

                    _userService.Update(user);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = _roleService.RoleSelectList(_roleService.GetAll());

            return View(user);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = _userService.GetByID(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = _userService.GetByID(id);
            _userService.Delete(user);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "Register")]
        public async Task<IActionResult> Register([Bind("FirstName,LastName,Email,PhoneNumber,Password,ConfirmPassword")] RegisterViewModel model)
        {
            User registerUser = new User();
            if (ModelState.IsValid)
            {
                if (EmailAreadyExist(model.Email))
                {
                    ModelState.AddModelError("Email", "Email already exist !");
                }
                else
                {
                    registerUser.UserName = model.FirstName + model.LastName;
                    registerUser.Email = model.Email;
                    registerUser.Phone = model.PhoneNumber;
                    registerUser.Salt = Utilities.GetRandomKey();
                    registerUser.LastLogin = null;
                    registerUser.Passwd = (model.Password + registerUser.Salt).ToMD5();

                    int defaultRoleID = _roleService.GetDefaultRoleID();
                    registerUser.RoleId = defaultRoleID;

                    _userService.Insert(registerUser);

                    TempData["shortMessage"] = "Success - Login to continue !";

                    return RedirectToAction("Login", "User", new { Area = "Admin" });
                }
            }
            return View(model);
        }

        public IActionResult Filtter(int roleID = 0)
        {
            var url = "";
            if (roleID == 0)
            {
                url = $"/admin/User/Index?roleID={0}";
            }
            else
            {
                url = $"/admin/User/Index?roleID={roleID}";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        private bool EmailAreadyExist(string email)
        {
            var validateEmailName = _userService.GetByEmail(email);

            if (validateEmailName != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}