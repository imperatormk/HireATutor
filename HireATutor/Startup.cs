using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HireATutor.Startup))]
namespace HireATutor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
