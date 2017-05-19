using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using NGODirectory.Backend.DataObjects;
using NGODirectory.Backend.Models;

namespace NGODirectory.Backend.Controllers
{
    public class OrganizationController : TableController<Organization>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Organization>(context, Request);
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
            return UpdateAsync(id, patch);
        }

        [Authorize]
        public async Task<IHttpActionResult> PostOrganization(Organization item)
        {
            Organization current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        [Authorize]
        public Task DeleteOrganization(string id)
        {
            return DeleteAsync(id);
        }
    }
}