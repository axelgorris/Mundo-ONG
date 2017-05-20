using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using NGODirectory.Backend.DataObjects;
using NGODirectory.Backend.Models;
using NGODirectory.Backend.Helpers;
using NGODirectory.Backend.Attributes;
using System.Security.Claims;
using NGODirectory.Backend.Extensions;
using System.Net;

namespace NGODirectory.Backend.Controllers
{
    [EnableQuery(PageSize = 5)]
    public class OrganizationController : TableController<Organization>
    {
        public string UserId
        {
            get
            {
                var principal = this.User as ClaimsPrincipal;
                return principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
        }

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

        //[AuthorizeClaims("groups", Locations.OrganizationOwnersGroupId)]
        [Authorize]
        public Task<Organization> PatchOrganization(string id, Delta<Organization> patch)
        {
            ValidateOrganizationAdmin(id);

            return UpdateAsync(id, patch);
        }

        //[AuthorizeClaims("groups", Locations.OrganizationOwnersGroupId)]
        [Authorize]
        public async Task<IHttpActionResult> PostOrganization(Organization item)
        {
            item.AdminUser = UserId;
            Organization current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        //[AuthorizeClaims("groups", Locations.OrganizationOwnersGroupId)]
        [Authorize]
        public Task DeleteOrganization(string id)
        {
            ValidateOrganizationAdmin(id);

            return DeleteAsync(id);
        }
    }
}