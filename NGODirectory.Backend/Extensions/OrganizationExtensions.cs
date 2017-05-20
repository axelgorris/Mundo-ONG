using NGODirectory.Backend.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NGODirectory.Backend.Extensions
{
    public static class OrganizationExtensions
    {
        public static IQueryable<Organization> IsOrgzanitionAdminFilter(this IQueryable<Organization> query, string userid)
        {
            return query.Where(item => item.AdminUser.Equals(userid));
        }
    }
}