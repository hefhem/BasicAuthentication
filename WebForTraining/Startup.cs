using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebForTraining.Startup))]
namespace WebForTraining
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
