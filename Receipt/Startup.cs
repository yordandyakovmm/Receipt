using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Receipt.Startup))]
namespace Receipt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
