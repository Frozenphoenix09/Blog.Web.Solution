using Blog.App.Data.Common;
using Blog.App.Data.Models;
using Blog.App.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.App.Service.Service
{
    public interface ICommentService : IEntityService<Comment> 
    { 
    }
    public class CommentService : EntityService<Comment> , ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        public CommentService(IUnitOfWork unitOfWork, ICommentRepository commentRepository) : base(unitOfWork,commentRepository)
        {
            _commentRepository = commentRepository;
        }
    }
}
