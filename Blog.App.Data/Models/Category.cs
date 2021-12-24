using System;
using System.Collections.Generic;

#nullable disable

namespace Blog.App.Data.Models
{
    public partial class Category
    {
        public Category()
        {
            InverseCatParent = new HashSet<Category>();
            Posts = new HashSet<Post>();
        }

        public int CatId { get; set; }
        public int? CatParentId { get; set; }
        public string CatName { get; set; }
        public string Alias { get; set; }
        public bool ShowOnHome { get; set; }
        public bool ShowOnMenu { get; set; }
        public int LayoutType { get; set; }
        public string LayoutDescription { get; set; }
        public int? Index { get; set; }

        public virtual Category CatParent { get; set; }
        public virtual ICollection<Category> InverseCatParent { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
