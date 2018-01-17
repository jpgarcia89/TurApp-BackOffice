using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TurApp.Startup))]
namespace TurApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
