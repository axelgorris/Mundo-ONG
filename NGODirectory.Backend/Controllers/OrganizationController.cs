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
    [EnableQuery(PageSize = 10)]
    public class OrganizationController : TableController<Organization>
    {
        public string UserId => ((ClaimsPrincipal)User).FindFirst(ClaimTypes.NameIdentifier).Value;
        
        public void ValidateOrganizationAdmin(string id)
        {
            var result = Lookup(id).Queryable.IsOrgzanitionAdminFilter(UserId).FirstOrDefault();
            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Organization>(context, Request, enableSoftDelete: true);
        }

        public IQueryable<Organization> GetAllOrganizations()
        {
            return Query();
        }

        public SingleResult<Organization> GetOrganization(string id)
        {
            return Lookup(id);
        }

        [Authorize]
        public Task<Organization> PatchOrganization(string id, Delta<Organization> patch)
        {
            ValidateOrganizationAdmin(id);
            
            return UpdateAsync(id, patch);
        }

        [Authorize]
        public async Task<IHttpActionResult> PostOrganization(Organization item)
        {
            item.AdminUser = UserId;
            Organization current = await InsertAsync(item);

            HubNotificationHelper hubNotification = new HubNotificationHelper(this.Configuration);
            bool messageSent = await hubNotification.SendPushNotification($"¡Damos la bienvenida a {item.Name}!");

            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }
        
        [Authorize]
        public Task DeleteOrganization(string id)
        {
            ValidateOrganizationAdmin(id);

            return DeleteAsync(id);
        }
    }
}