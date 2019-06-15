using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SistemaInventarioVentas.Startup))]
namespace SistemaInventarioVentas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
