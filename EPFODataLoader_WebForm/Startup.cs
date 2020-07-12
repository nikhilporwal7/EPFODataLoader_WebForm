using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EPFODataLoader_WebForm.Startup))]
namespace EPFODataLoader_WebForm
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
