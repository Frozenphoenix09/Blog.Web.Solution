using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.App.Data.Models;
using Blog.App.WebApp.Enums;

namespace Blog.App.WebApp.Controllers.Components
{

    public class NavViewComponent : ViewComponent
    {
        private readonly BlogDataBaseContext _context;
        private IMemoryCache _memoryCache;

        public NavViewComponent(BlogDataBaseContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public IViewComponentResult Invoke()
        {
            var categoryls = _memoryCache.GetOrCreate(CacheKeys.Categories, entry =>
            {
                entry.SlidingExpiration = TimeSpan.MaxValue;
                return GetCategoryLs();
            });

            return View(categoryls);
        }

        public List<Category> GetCategoryLs()
        {
            List<Category> categories = new List<Category>();
            categories = _context.Categories.Where(c => c.CatParentId == null && c.ShowOnMenu).Include(c => c.InverseCatParent).ToList();

            return categories;
        }

    }
}
