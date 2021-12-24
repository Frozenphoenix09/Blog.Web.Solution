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
    public interface IApprovePostService : IEntityService<ApprovePost>
    {

    }
    public class ApprovePostService : EntityService<ApprovePost> , IApprovePostService
    {
        private readonly IApprovePostRepository _approvePostRepository;
        public ApprovePostService(IUnitOfWork unitOfWork , IApprovePostRepository approvePostRepository) : base(unitOfWork,approvePostRepository)
        {
            _approvePostRepository = approvePostRepository;
        }
    }
}
