using System;
using System.Collections.Generic;

#nullable disable

namespace Blog.App.Data.Models
{
    public partial class Image
    {
        public int ImageId { get; set; }
        public int? PostId { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }

        public virtual Post Post { get; set; }
    }
}
