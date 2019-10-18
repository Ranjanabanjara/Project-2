using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Project_2.Startup))]
namespace Project_2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
