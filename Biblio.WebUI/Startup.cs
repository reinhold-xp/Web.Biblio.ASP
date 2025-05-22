using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Biblio.WebUI.Startup))]
namespace Biblio.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
