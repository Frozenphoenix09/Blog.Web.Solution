using Blog.App.Data.Common;
using Blog.App.Data.Models;
using Blog.App.Data.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Blog.App.Service.Service
{
    public interface IUserService : IEntityService<User>
    {
        public List<User> GetAllUser();
        public User GetByEmail(string email);
        public User GetByID(int id);
        public List<User> GetByRole(int roleID);

        public List<SelectListItem> UserSelectList(List<User> list);

    }

    public class UserService : EntityService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository) : base(unitOfWork, userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetByEmail(string email)
        {
            return _userRepository.GetByEmail(email);
        }

        public User GetByID(int id)
        {
            return _userRepository.GetByID(id);
        }

        public List<User> GetAllUser()
        {
            return _userRepository.GetAllUser();
        }

        public List<User> GetByRole(int roleID)
        {
            return _userRepository.GetByRole(roleID);
        }

        public List<SelectListItem> UserSelectList(List<User> list)
        {
            List<SelectListItem> userSelectList = new List<SelectListItem>();

            foreach (User item in list)
            {
                var listItem = new SelectListItem() { Text = item.Email, Value = item.UserId.ToString() };
                userSelectList.Add(listItem);
            }
            return userSelectList;
        }
    }
}