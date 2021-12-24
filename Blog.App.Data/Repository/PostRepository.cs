using Blog.App.Data.Common;
using Blog.App.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
namespace Blog.App.Data.Repository
{
    public interface IPostRepository : IBaseRepository<Post> 
    {
        public List<Post> GetAllPost();

        public List<Post> GetByCategory( int catID);

        public List<Post> GetByAuthor(int authorID);

        public List<Post> GetByAuthorAndCategory(int authorID, int catID);

        public Post GetByID(int id);

        public List<Post> GetByKeyWord(string keyword , int ? authorid );

    }
    public class PostRepository : BaseRepository<Post> , IPostRepository
    {
        public PostRepository(BlogDataBaseContext context): base(context)
        {
                
        }
        public List<Post> GetAllPost()
        {
            return Dbset.AsNoTracking().Include(p => p.Author).Include(p => p.Cat).ToList();
        }

        public List<Post> GetByAuthor(int authorID)
        {
            return Dbset.AsNoTracking().Include(p => p.Author).Include(p => p.Cat).Where(p=>p.AuthorId == authorID).ToList();
        }

        public List<Post> GetByAuthorAndCategory(int authorID, int catID)
        {
            return Dbset.AsNoTracking().Include(p => p.Author).Include(p => p.Cat).Where(p => p.AuthorId == authorID && p.CatId == catID).ToList();
        }

        public List<Post> GetByCategory(int catID)
        {
            return Dbset.AsNoTracking().Include(p => p.Author).Include(p => p.Cat).Where(p=>p.CatId == catID).ToList();
        }

        public Post GetByID( int id )
        {
            return Dbset.AsNoTracking().Include(p => p.Author).Include(p => p.Cat).FirstOrDefault(p => p.PostId == id);
        }

        public List<Post> GetByKeyWord(string keyword , int ? authorid)
        {
            if(authorid != null)
            {
                return Dbset.AsNoTracking()
                            .Include(p => p.Cat)
                            .Include(p => p.Author)
                            .Where(p => p.Title.Contains(keyword.ToLower()) || p.Content.Contains(keyword.ToLower()))
                            .Where(p => p.AuthorId == authorid)
                            .OrderByDescending(p => p.DateCreate)
                            .ToList();
            }
            return Dbset.AsNoTracking()
                               .Include(p => p.Cat)
                               .Include(p => p.Author)
                               .Where(p => p.Title.Contains(keyword.ToLower()) || p.Content.Contains(keyword.ToLower()))
                               .OrderByDescending(p => p.DateCreate)
                               .ToList();
        }
    }
}
