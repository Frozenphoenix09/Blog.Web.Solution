using Blog.App.Data.Common;
using Blog.App.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.App.Data.Repository
{
    public interface ICommentRepository : IBaseRepository<Comment> { }
    public class CommentRepository : BaseRepository<Comment> , ICommentRepository
    {
        public CommentRepository(BlogDataBaseContext context) : base (context)
        {
        }
    }
}
