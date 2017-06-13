using Microsoft.Azure.Mobile.Server;
using NGODirectory.Backend.DataObjects;
using NGODirectory.Backend.Extensions;
using NGODirectory.Backend.Helpers;
using NGODirectory.Backend.Models;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

namespace NGODirectory.Backend.Controllers
{
    public class AnnouncementController : TableController<Announcement>
    {
        public string UserId => ((ClaimsPrincipal)User).FindFirst(ClaimTypes.NameIdentifier).Value;

        public void ValidateAnnouncementAuthor(string id)
        {
            var result = Lookup(id).Queryable.IsAnnouncementAuthorFilter(UserId).FirstOrDefault();
            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Announcement>(context, Request, enableSoftDelete: true);
        }

        public IQueryable<Announcement> GetAllAnnouncements()
        {
            return Query();
        }

        public SingleResult<Announcement> GetAnnouncement(string id)
        {
            return Lookup(id);
        }

        [Authorize]
        public Task<Announcement> PatchAnnouncement(string id, Delta<Announcement> patch)
        {
            ValidateAnnouncementAuthor(id);

            return UpdateAsync(id, patch);
        }

        [Authorize]
        public async Task<IHttpActionResult> PostAnnouncement(Announcement item)
        {
            item.Author = UserId;
            Announcement current = await InsertAsync(item);

            HubNotificationHelper hubNotification = new HubNotificationHelper(this.Configuration);
            bool messageSent = await hubNotification.SendPushNotification(item.Title);

            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        [Authorize]
        public Task DeleteAnnouncement(string id)
        {
            ValidateAnnouncementAuthor(id);

            return DeleteAsync(id);
        }
    }
}