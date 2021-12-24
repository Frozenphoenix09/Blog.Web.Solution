using System;
using System.Collections.Generic;

#nullable disable

namespace Blog.App.Data.Models
{
    public partial class User
    {
        public User()
        {
            ApprovePosts = new HashSet<ApprovePost>();
            Comments = new HashSet<Comment>();
            Posts = new HashSet<Post>();
        }

        public int UserId { get; set; }
        public int? RoleId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Passwd { get; set; }
        public string Status { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Salt { get; set; }
        public string Thumb { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<ApprovePost> ApprovePosts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
