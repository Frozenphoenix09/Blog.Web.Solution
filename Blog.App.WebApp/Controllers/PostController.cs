using Blog.App.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.App.WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly BlogDataBaseContext _context;

        public PostController(BlogDataBaseContext context)
        {
            _context = context;
        }
        [Route("{Alias}", Name = "ListPost")]
        // GET: Posts
        public async Task<IActionResult> List(string Alias)
        {

            if (string.IsNullOrEmpty(Alias)) return RedirectToAction("Home", "Index");
            var category = _context.Categories.FirstOrDefault(c => c.Alias == Alias);

            if (category == null) return RedirectToAction("Home", "Index");

            var lsPost = _context.Posts.Include(p => p.Cat).Include(p => p.Author).AsNoTracking().Where(p => p.Cat.CatName == category.CatName).ToList();

            ViewData["Category"] = category.CatName;
            return View(lsPost);
        }

        [Route("/{Alias}.html", Name = "PostDetails")]
        // GET: Posts/Details/5
        public async Task<IActionResult> Details(string Alias)
        {
            if (string.IsNullOrEmpty(Alias))
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Cat)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Alias == Alias);

            if (post == null)
            {
                return NotFound();
            }
            ViewData["Comments"] = _context.Comments.AsNoTracking().Include(c => c.InverseCommentParent).Where(c => c.PostId == post.PostId && c.CommentParent == null).ToList();
            post.ViewCount += 1;
            _context.Update(post);
            await _context.SaveChangesAsync();
            return View(post);
        }   
    }
}
