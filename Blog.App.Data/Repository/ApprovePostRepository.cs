using Blog.App.Data.Common;
using Blog.App.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.App.Data.Repository
{
    public interface IApprovePostRepository : IBaseRepository<ApprovePost> 
    { 
    }
    public class ApprovePostRepository : BaseRepository<ApprovePost> , IApprovePostRepository
    {
        public ApprovePostRepository(BlogDataBaseContext context) : base(context)
        {
        }
    }
}
