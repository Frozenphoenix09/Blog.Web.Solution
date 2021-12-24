using Blog.App.Data.Common;
using Blog.App.Data.Models;
using Blog.App.Data.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.App.Service.Service
{
    public interface IImageService : IEntityService<Image> 
    {
        public bool CreateImage(int postID, string name, string path);

        public bool UpdateImage(int postID, string name, string path);

        public Image GetByPostID( int id);
    }
    class ImageService : EntityService<Image> , IImageService
    {
        private readonly IImageRepository _imageRepository;
        public ImageService(IUnitOfWork unitOfWork , IImageRepository imageRepository) : base(unitOfWork, imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public bool CreateImage( int postID , string name , string path)
        {
            try
            {
                Image image = new Image();
                image.PostId = postID;
                image.ImageName = name;
                image.ImagePath = path;

                _imageRepository.Insert(image);
                UnitOfWork.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }            
        }

        public Image GetByPostID( int id )
        {
            return _imageRepository.GetByPostID(id);
        }

        public bool UpdateImage(int postID, string name, string path)
        {
            try
            {
                Image image = _imageRepository.GetByPostID(postID);
                image.ImageName = name;
                image.ImagePath = path;

                _imageRepository.Update(image);
                UnitOfWork.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
