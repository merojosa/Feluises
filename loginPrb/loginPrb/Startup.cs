using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(loginPrb.Startup))]
namespace loginPrb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
