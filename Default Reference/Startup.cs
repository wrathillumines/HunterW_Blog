using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Default_Reference.Startup))]
namespace Default_Reference
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
