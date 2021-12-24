using Blog.App.Data.Common;
using Blog.App.Data.Models;
using Blog.App.Data.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Blog.App.Service.Service
{
    public interface IRoleService : IEntityService<Role>
    {
        public List<SelectListItem> RoleSelectList(List<Role> list);

        public int GetDefaultRoleID();

        public bool CreateRole(Role role);

        public bool IsRoleAlreadyExist(string roleName);

    }

    public class RoleService : EntityService<Role>, IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IUnitOfWork unitOfWork, IRoleRepository roleRepository) : base(unitOfWork, roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public bool CreateRole(Role role)
        {
            try
            {
                _roleRepository.Insert(role);
                UnitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Post GetByID()
        {
            throw new NotImplementedException();
        }
        public int GetDefaultRoleID()
        {
            return _roleRepository.GetDefaultRoleID();
        }

        public bool IsRoleAlreadyExist(string roleName)
        {
            return _roleRepository.IsRoleAlreadyExist(roleName);
        }

        public List<SelectListItem> RoleSelectList(List<Role> list)
        {
            List<SelectListItem> roleList = new List<SelectListItem>();

            foreach (Role item in list)
            {
                var listItem = new SelectListItem() { Text = item.RoleName, Value = item.RoleId.ToString() };
                roleList.Add(listItem);
            }
            return roleList;
        }
    }
}