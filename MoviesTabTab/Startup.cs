using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MoviesTabTab.Startup))]
namespace MoviesTabTab
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
