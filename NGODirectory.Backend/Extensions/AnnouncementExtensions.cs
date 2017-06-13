using NGODirectory.Backend.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NGODirectory.Backend.Extensions
{
    public static class AnnouncementExtensions
    {
        public static IQueryable<Announcement> IsAnnouncementAuthorFilter(this IQueryable<Announcement> query, string userid)
        {
            return query.Where(item => item.Author.Equals(userid));
        }
    }
}