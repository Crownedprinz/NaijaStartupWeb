using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NaijaStartupWeb.Startup))]
namespace NaijaStartupWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
