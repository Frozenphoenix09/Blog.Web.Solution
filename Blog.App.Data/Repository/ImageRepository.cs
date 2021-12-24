using Blog.App.Data.Common;
using Blog.App.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.App.Data.Repository
{
    public interface IImageRepository : IBaseRepository<Image> 
    {
        public Image GetByPostID(int id);
    }
    public class ImageRepository : BaseRepository<Image> , IImageRepository
    {
        public ImageRepository(BlogDataBaseContext context ) : base (context)
        {
        }

        public Image GetByPostID(int id)
        {
            return Dbset.AsNoTracking().Include(i => i.Post).FirstOrDefault(i => i.PostId == id);
        }
    }
}
