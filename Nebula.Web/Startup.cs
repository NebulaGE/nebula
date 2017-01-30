using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Nebula.Web.Startup))]
namespace Nebula.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
