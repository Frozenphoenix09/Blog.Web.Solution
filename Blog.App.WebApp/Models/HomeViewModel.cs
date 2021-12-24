using Blog.App.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.App.WebApp.Models
{
    public class HomeViewModel
    {
        public List<Post> CategoryPosts { get; set; }

        public Category Category { get; set; }
    }
}
