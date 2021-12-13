using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HMTStationery.Startup))]
namespace HMTStationery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
