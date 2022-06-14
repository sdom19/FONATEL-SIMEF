using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GB.SUTEL.UI.Startup))]
namespace GB.SUTEL.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
