using Blog.App.Data.Common;
using Blog.App.Data.Models;
using System.Linq;

namespace Blog.App.Data.Repository
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        public bool IsCategoryExist(string catName);
    }

    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(BlogDataBaseContext context) : base(context)
        {
        }

        public bool IsCategoryExist(string catName)
        {
            if (Dbset.Any(c => c.CatName == catName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}