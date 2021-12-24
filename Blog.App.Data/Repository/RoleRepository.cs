using Blog.App.Data.Common;
using Blog.App.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Blog.App.Data.Repository
{
    public interface IRoleRepository : IBaseRepository<Role> 
    {
        public int GetDefaultRoleID();

        public bool IsRoleAlreadyExist(string roleName);


    }
    public class RoleRepository : BaseRepository<Role> , IRoleRepository
    {
        public RoleRepository(BlogDataBaseContext context) : base(context)
        {

        }

        public int GetDefaultRoleID()
        {
            return Dbset.AsNoTracking().FirstOrDefault(r => r.RoleName == "Normal User").RoleId;

        }

        public bool IsRoleAlreadyExist(string roleName)
        {
            if(Dbset.AsNoTracking().Any(r=>r.RoleName == roleName))
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
