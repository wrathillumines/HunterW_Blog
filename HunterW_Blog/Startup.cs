using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HunterW_Blog.Startup))]
namespace HunterW_Blog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
