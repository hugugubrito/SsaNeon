using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PfcSsaNeon.Startup))]
namespace PfcSsaNeon
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
