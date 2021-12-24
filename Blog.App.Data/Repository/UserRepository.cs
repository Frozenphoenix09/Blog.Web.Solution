using Blog.App.Data.Common;
using Blog.App.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Blog.App.Data.Repository
{
    public interface IUserRepository : IBaseRepository<User> 
    {
        public List<User> GetAllUser();
        public User GetByEmail(string email);
        public User GetByID(int id);

        public List<User> GetByRole(int roleID);
    }
    public class UserRepository : BaseRepository<User> , IUserRepository
    {
        public UserRepository(BlogDataBaseContext context) : base(context)
        {

        }

        public User GetByEmail(string email)
        {
            return Dbset.AsNoTracking().Include(u => u.Role).FirstOrDefault(u => u.Email.ToLower().Trim() == email.ToLower().Trim());
        }

        public User GetByID(int id)
        {
            return Dbset.AsNoTracking().Include(u => u.Role).FirstOrDefault(u => u.UserId == id);
        }

        public List<User> GetAllUser()
        {
            return Dbset.AsNoTracking().Include(u => u.Role).ToList();
        }

        public List<User> GetByRole(int roleID)
        {
            return Dbset.AsNoTracking().Include(u => u.Role).Where(u => u.Role.RoleId == roleID).ToList();
        }
    }
}
