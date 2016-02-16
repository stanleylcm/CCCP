using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CCCP.Startup))]
namespace CCCP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
