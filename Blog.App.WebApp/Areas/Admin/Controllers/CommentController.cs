using Blog.App.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.App.WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    public class CommentController : Controller
    {
        private readonly BlogDataBaseContext _context;

        public CommentController(BlogDataBaseContext context)
        {
            _context = context;
        }

        // POST: admin/Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int postId, string email, string alias, string content)
        {
            string url = $"/{alias}.html";

            Comment comment = new Comment();

            comment.Email = email;
            comment.DateCreate = DateTime.Now;
            comment.PostId = postId;
            comment.Content = content;

            _context.Add(comment);
            await _context.SaveChangesAsync();

            return Redirect(url);
        }

        [HttpGet]
        public IActionResult CreateAReplyComment(string commentParentID, string alias, string postID)
        {
            ViewData["CommentParentID"] = commentParentID;
            ViewData["Alias"] = alias;
            ViewData["PostID"] = postID;
            return PartialView("_NewComment");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAReplyComment(int commentParentID, string alias, string content, string email, int postID)
        {
            string url = $"/{alias}.html";

            Comment replyComment = new Comment();
            replyComment.PostId = postID;
            replyComment.DateCreate = DateTime.Now;
            replyComment.CommentParentId = commentParentID;
            replyComment.Content = content;
            replyComment.Email = email;

            _context.Add(replyComment);
            await _context.SaveChangesAsync();

            return Redirect(url);
        }
    }
}
