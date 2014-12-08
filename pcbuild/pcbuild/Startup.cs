using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(pcbuild.Startup))]
namespace pcbuild
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
