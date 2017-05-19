using Microsoft.Azure.Mobile.Server;
using NGODirectory.Backend.DataObjects;
using NGODirectory.Backend.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

namespace NGODirectory.Backend.Controllers
{
    public class AnnouncementController : TableController<Announcement>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Announcement>(context, Request);
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
            return UpdateAsync(id, patch);
        }

        [Authorize]
        public async Task<IHttpActionResult> PostAnnouncement(Announcement item)
        {
            Announcement current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        [Authorize]
        public Task DeleteAnnouncement(string id)
        {
            return DeleteAsync(id);
        }
    }
}