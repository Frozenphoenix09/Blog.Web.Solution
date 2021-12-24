using Blog.App.Data.Common;
using Blog.App.Data.Models;
using Blog.App.Data.Repository;
using Blog.App.Service.Helper;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace Blog.App.Service.Service
{
    public interface IPostService : IEntityService<Post>
    {
        public List<Post> GetAllPost();

        public List<Post> GetByCategory(int catID);

        public List<Post> GetByAuthor(int authorID);

        public List<Post> GetByAuthorAndCategory(int authorID, int catID);

        public Post GetByID(int id);

        public bool CreatePost(Post post, int authorID , string ? thumb);

        public bool UpdatePost(Post post, int authorID, string? thumb);

        public List<Post> GetByKeyWord(string keyword, int? authorID);
    }

    public class PostService : EntityService<Post>, IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IUnitOfWork unitOfWork, IPostRepository postRepository) : base(unitOfWork, postRepository)
        {
            _postRepository = postRepository;
        }

        public bool CreatePost(Post post, int authorID ,string ? thumb)
        {
            try
            {
                post.AuthorId = authorID;
                post.Alias = Utilities.SEOUrl(post.Title);
                post.ViewCount = 0;
                post.Thumb = thumb;
                
                _postRepository.Insert(post);
                UnitOfWork.SaveChanges();

                return true;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public List<Post> GetAllPost()
        {
            return _postRepository.GetAllPost();
        }

        public List<Post> GetByAuthor(int authorID)
        {
            return _postRepository.GetByAuthor(authorID);
        }

        public List<Post> GetByAuthorAndCategory(int authorID, int catID)
        {
            return _postRepository.GetByAuthorAndCategory(authorID, catID);
        }

        public List<Post> GetByCategory(int catID)
        {
            return _postRepository.GetByCategory(catID);
        }

        public Post GetByID(int id)
        {
            return _postRepository.GetByID(id);
        }

        public List<Post> GetByKeyWord(string keyword, int? authorID)
        {
            return _postRepository.GetByKeyWord(keyword, authorID);
        }

        public bool UpdatePost(Post post, int authorID, string? thumb)
        {
            try
            {
                post.AuthorId = authorID;
                post.Alias = Utilities.SEOUrl(post.Title);
                post.Thumb = thumb;

                _postRepository.Update(post);
                UnitOfWork.SaveChanges();

                return true;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}