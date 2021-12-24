using System;
using System.Collections.Generic;

#nullable disable

namespace Blog.App.Data.Models
{
    public partial class Post
    {
        public Post()
        {
            ApprovePosts = new HashSet<ApprovePost>();
            Comments = new HashSet<Comment>();
            Images = new HashSet<Image>();
        }

        public int PostId { get; set; }
        public int? AuthorId { get; set; }
        public int? CatId { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string Title { get; set; }
        public string Thumb { get; set; }
        public string Alias { get; set; }
        public string AuthorName { get; set; }
        public string Status { get; set; }
        public DateTime DateCreate { get; set; }
        public int ViewCount { get; set; }

        public virtual User Author { get; set; }
        public virtual Category Cat { get; set; }
        public virtual ICollection<ApprovePost> ApprovePosts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
