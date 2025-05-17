using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Biblio.Mvc.Startup))]
namespace Biblio.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
