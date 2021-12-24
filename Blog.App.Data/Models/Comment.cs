using System;
using System.Collections.Generic;

#nullable disable

namespace Blog.App.Data.Models
{
    public partial class Comment
    {
        public Comment()
        {
            InverseCommentParent = new HashSet<Comment>();
        }

        public int CommentId { get; set; }
        public int? UserId { get; set; }
        public int? PostId { get; set; }
        public DateTime DateCreate { get; set; }
        public string Content { get; set; }
        public int? CommentParentId { get; set; }
        public string AuthorName { get; set; }
        public string Email { get; set; }

        public virtual Comment CommentParent { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> InverseCommentParent { get; set; }
    }
}
