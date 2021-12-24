using System;
using System.Collections.Generic;

#nullable disable

namespace Blog.App.Data.Models
{
    public partial class ApprovePost
    {
        public int Id { get; set; }
        public int? PostId { get; set; }
        public int? ApproverId { get; set; }
        public DateTime DateApprove { get; set; }

        public virtual User Approver { get; set; }
        public virtual Post Post { get; set; }
    }
}
