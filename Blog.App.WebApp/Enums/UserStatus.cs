using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.App.WebApp.Enums
{
    public enum PostStatus
    {
        Pending,
        Posted,
        Deleted
    }

    public enum UserStatus
    {
        Active,
        Inactive
    }

    public enum CacheKeys
    {
        Categories,
        Popuplar,
        Social
    }
}
