using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NGODirectory.Backend.Startup))]

namespace NGODirectory.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}