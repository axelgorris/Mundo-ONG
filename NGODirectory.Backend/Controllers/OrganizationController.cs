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

namespace NGODirectory.Backend.Controllers
{
    [EnableQuery(PageSize = 5)]
    public class OrganizationController : TableController<Organization>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Organization>(context, Request, enableSoftDelete:true);
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
            return UpdateAsync(id, patch);
        }

        //[AuthorizeClaims("groups", Locations.OrganizationOwnersGroupId)]
        [Authorize]
        public async Task<IHttpActionResult> PostOrganization(Organization item)
        {
            Organization current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        //[AuthorizeClaims("groups", Locations.OrganizationOwnersGroupId)]
        [Authorize]
        public Task DeleteOrganization(string id)
        {
            return DeleteAsync(id);
        }        
    }
}