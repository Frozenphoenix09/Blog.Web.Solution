using Blog.App.Data.Models;
using Blog.App.Service.Service;
using Blog.App.WebApp.Enums;
using Blog.App.WebApp.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.App.WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize()]
    public class PostController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly ICategoryService _catgoryService;
        private readonly IImageService _imageService;
        private readonly IApprovePostService _approvePostService;

        public PostController(IUserService userService , IPostService postService , ICategoryService categoryService , IImageService imageService , IApprovePostService approvePostService)
        {
            _userService = userService;
            _postService = postService;
            _catgoryService = categoryService;
            _imageService = imageService;
            _approvePostService = approvePostService;
        }

        public IActionResult Index( int catID = 0)
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var userID = HttpContext.Session.GetString("UserID");
            if (userID == null) return RedirectToAction("Login", "User", new { Area = "Admin" });

            var user = _userService.GetByID(int.Parse(userID));
            if (user == null) return NotFound();

            List<Post> posts = new List<Post>();

            if (user.Role != null && user.Role.IsAdmin == true)
            {
                if (catID != 0)
                {
                    posts = _postService.GetByCategory(catID);
                }
                else if (catID == 0)
                {
                    posts = _postService.GetAllPost();
                }
            }
            else if (user.Role != null && user.Role.IsAdmin == false)
            {
                if (catID != 0)
                {
                    posts = _postService.GetByAuthorAndCategory(user.UserId, catID);
                }
                else if (catID == 0)
                {
                    posts = _postService.GetByAuthor(user.UserId);
                }
            }
            ViewData["Category"] = _catgoryService.CatSelectList(_catgoryService.GetAll());
            return View(posts);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

            var userID = HttpContext.Session.GetString("UserID");
            if (userID == null) return RedirectToAction("Login", "User", new { Area = "Admin" });

            var user = _userService.GetByID(int.Parse(userID));
            if (user == null) return NotFound();

            var post = _postService.GetByID(id);

            if (post == null)
            {
                return NotFound();
            }

            if (user.Role != null && user.Role.IsAdmin == true)
            {
                return View(post);
            }
            else if (user.Role != null && user.Role.IsAdmin == false)
            {
                if (post.AuthorId != null && post.AuthorId != user.UserId)
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (post.AuthorId != null && post.AuthorId == user.UserId)
                {
                    return View(post);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else if (user.Role == null)
            {
                if (post.AuthorId != null && post.AuthorId != user.UserId)
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (post.AuthorId != null && post.AuthorId == user.UserId)
                {
                    return View(post);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var userID = HttpContext.Session.GetString("UserID");
            if (userID == null) return RedirectToAction("Login", "User", new { Area = "Admin" });

            ViewData["CatId"] = _catgoryService.CatSelectList(_catgoryService.GetAll());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Post post, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var userID = HttpContext.Session.GetString("UserID");
            if (userID == null) return RedirectToAction("Login", "User", new { Area = "Admin" });

            var user = _userService.GetByID(int.Parse(userID));
            if (user == null) return NotFound();

            if (ModelState.IsValid)
            {
                if(post.CatId == 0)
                {
                    ModelState.AddModelError("CatID", "Category can't be null");
                }
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string Newname = Utilities.SEOUrl(post.Title) + extension;
                    string thumb = await Utilities.UploadFile(fThumb, @"news\", Newname.ToLower());
                    string path = @"~/images/news/" + Newname;


                    _postService.CreatePost(post, user.UserId, thumb);

                    _imageService.CreateImage(post.PostId, Newname, path);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _postService.CreatePost(post,user.UserId, null);
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["CatId"] = _catgoryService.CatSelectList(_catgoryService.GetAll());
            return View(post);
        }

        public async Task<IActionResult> Edit(int id)
        {

            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var userID = HttpContext.Session.GetString("UserID");
            if (userID == null) return RedirectToAction("Login", "Users", new { Area = "Admin" });

            var user = _userService.GetByID(int.Parse(userID));
            if (user == null) return NotFound();

            var post = _postService.GetByID(id);

            var enumData = from PostStatus e in Enum.GetValues(typeof(PostStatus))
                           select new
                           {
                               Text = e,
                               Value = e
                           };

            ViewData["PostStatus"] = new SelectList(enumData, "Value", "Text", post.Status);

            ViewData["AuthorId"] = _userService.UserSelectList(_userService.GetAll());

            ViewData["CatId"] = _catgoryService.CatSelectList(_catgoryService.GetAll());


            if (post == null)
            {
                return NotFound();
            }

            if (user.Role != null && user.Role.IsAdmin == true)
            {
                return View(post);
            }
            else if (user.Role != null && user.Role.IsAdmin == false)
            {
                if (post.AuthorId != null && post.AuthorId != user.UserId)
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (post.AuthorId != null && post.AuthorId == user.UserId)
                {
                    return View(post);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else if (user.Role == null)
            {
                if (post.AuthorId != null && post.AuthorId != user.UserId)
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (post.AuthorId != null && post.AuthorId == user.UserId)
                {
                    return View(post);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( Post post,IFormFile fThumb)
        {

            // if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var userID = HttpContext.Session.GetString("UserID");
            if (userID == null) return RedirectToAction("Login", "Users", new { Area = "Admin" });

            var user = _userService.GetByID(int.Parse(userID));
            if (user == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    post.Alias = Utilities.SEOUrl(post.Title);

                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string Newname = Utilities.SEOUrl(post.Title) + extension;
                        string thumb = await Utilities.UploadFile(fThumb, @"news\", Newname.ToLower());

                        string path = @"~/images/news/" + Newname;


                        _postService.UpdatePost(post, user.UserId, thumb);

                        _imageService.UpdateImage(post.PostId, Newname, path);

                        return RedirectToAction(nameof(Index));

                    }
                    _postService.UpdatePost(post, user.UserId, post.Thumb);
                }
                catch (Exception ex)
                {                    
                    throw ex;                    
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["AuthorId"] = _userService.UserSelectList(_userService.GetAll());
            ViewData["CatId"] = _catgoryService.CatSelectList(_catgoryService.GetAll());
            return View(post);
        }


        public async Task<IActionResult> Delete(int id)
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var userID = HttpContext.Session.GetString("UserID");
            if (userID == null) return RedirectToAction("Login", "Users", new { Area = "Admin" });

            var user = _userService.GetByID(int.Parse(userID));
            if (user == null) return NotFound();

            var post = _postService.GetByID(id);

            if (post == null)
            {
                return NotFound();
            }

            if (user.Role != null && user.Role.IsAdmin == true)
            {
                return View(post);
            }
            else if (user.Role != null && user.Role.IsAdmin == false)
            {
                if (post.AuthorId != null && post.AuthorId == user.UserId)
                {
                    return View(post);
                }
                else if (post.AuthorId != null && post.AuthorId != user.UserId)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else if (user.Role == null)
            {
                if (post.AuthorId != null && post.AuthorId == user.UserId)
                {
                    return View(post);
                }
                else if (post.AuthorId != null && post.AuthorId != user.UserId)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var post =  _postService.GetByID(id);
            _postService.Delete(post);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Filtter(int catID = 0)
        {
            var url = "";
            if (catID == 0)
            {
                url = $"/admin/Post/Index?catID={0}";
            }
            else
            {
                url = $"/admin/Post/Index?catID={catID}";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        public IActionResult SearchForPost(string keyword)
        {
            var result = new List<Post>();
            var userID = HttpContext.Session.GetString("UserID");
            if (userID == null) return RedirectToAction("Login", "Users", new { Area = "Admin" });

            var user = _userService.GetByID(int.Parse(userID));
            if (user == null) return NotFound();

            try
            {
                if (keyword != null && keyword.Trim().Length >= 3)
                {
                    if (user.Role != null && user.Role.IsAdmin == true)
                    {
                        result = _postService.GetByKeyWord(keyword, null);
                    }
                    else if (user.Role != null && user.Role.IsAdmin == false)
                    {
                        result = _postService.GetByKeyWord(keyword, user.UserId);
                    }
                }
                return PartialView("_PostResult", result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ApprovePost(int id)
        {
            var postToApprove = _postService.GetByID (id);
            return View(postToApprove);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ApprovePost(int id, int userID)
        {
            try
            {
                var postToApprove = _postService.GetByID(id);
                postToApprove.Status = "Posted";

                ApprovePost approvePost = new ApprovePost();
                approvePost.DateApprove = DateTime.Now;
                approvePost.PostId = id;
                approvePost.ApproverId = userID;

                _postService.Update(postToApprove);
                _approvePostService.Insert(approvePost);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
