using Blog.App.Data.Common;
using Blog.App.Data.Models;
using Blog.App.Data.Repository;
using Blog.App.Service.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Blog.App.Service.Service
{
    public interface ICategoryService : IEntityService<Category> 
    {
        public bool CreateCategory(Category model);
        public List<SelectListItem> CatSelectList(List<Category> list);

        public bool IsCategoryExist(string catName);

    }
    class CategoryService : EntityService<Category> , ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(IUnitOfWork unitOfWork , ICategoryRepository categoryRepository) : base (unitOfWork, categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<SelectListItem> CatSelectList(List<Category> list)
        {
            List<SelectListItem> catPrentList = new List<SelectListItem>();

            foreach ( Category item in list)
            {
                var listItem = new SelectListItem() { Text = item.CatName, Value = item.CatId.ToString() };
                catPrentList.Add(listItem);
            }
            return catPrentList;
        }

        public bool CreateCategory(Category model)
        {
            model.Alias = Utilities.SEOUrl(model.CatName);

            try
            {
                _categoryRepository.Insert(model);
                UnitOfWork.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        public bool IsCategoryExist(string catName)
        {
            return _categoryRepository.IsCategoryExist(catName);
        }
    }
}
