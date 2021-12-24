using Blog.App.Data.Models;
using Blog.App.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.App.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogDataBaseContext _context;

        public HomeController(ILogger<HomeController> logger , BlogDataBaseContext context )
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["LastestPost"] = _context.Posts.AsNoTracking().Include(p => p.Author).Include(p => p.Cat).Where(p => p.Status == "Posted").OrderByDescending(p => p.DateCreate).Take(1).ToList();
            ViewData["Populars"] = _context.Posts.AsNoTracking().Include(p => p.Author).Include(p => p.Cat).Where(p => p.Status == "Posted").OrderByDescending(p => p.ViewCount).Take(5).ToList();
            ViewData["Recents"] = _context.Posts.AsNoTracking().Include(p => p.Author).Include(p => p.Cat).Where(p => p.Status == "Posted").OrderByDescending(p => p.DateCreate).Take(5).ToList();

            List<HomeViewModel> homeViewModels = new List<HomeViewModel>();

            List<Category> categories = _context.Categories.AsNoTracking().Where(c => c.ShowOnHome == true).ToList();

            foreach (var item in categories)
            {
                if (item.LayoutType == 1)
                {
                    HomeViewModel homeViewModel = new HomeViewModel();
                    homeViewModel.Category = item;
                    homeViewModel.CategoryPosts = _context.Posts.AsNoTracking().Include(p => p.Cat).Include(p => p.Author).Where(p => p.CatId == item.CatId && p.Status == "Posted").OrderByDescending(p => p.ViewCount).Take(5).ToList();
                    homeViewModels.Add(homeViewModel);
                }
                if (item.LayoutType == 2)
                {
                    HomeViewModel homeViewModel = new HomeViewModel();
                    homeViewModel.Category = item;
                    homeViewModel.CategoryPosts = _context.Posts.AsNoTracking().Include(p => p.Cat).Include(p => p.Author).Where(p => p.CatId == item.CatId && p.Status == "Posted").OrderByDescending(p => p.ViewCount).Take(6).ToList();
                    homeViewModels.Add(homeViewModel);
                }
                if (item.LayoutType == 3)
                {
                    HomeViewModel homeViewModel = new HomeViewModel();
                    homeViewModel.Category = item;
                    homeViewModel.CategoryPosts = _context.Posts.AsNoTracking().Include(p => p.Cat).Include(p => p.Author).Where(p => p.CatId == item.CatId && p.Status == "Posted").OrderByDescending(p => p.ViewCount).Take(4).ToList();
                    homeViewModels.Add(homeViewModel);
                }
            }
            return View(homeViewModels);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
