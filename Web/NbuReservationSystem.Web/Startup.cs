using Microsoft.Owin;

using Owin;

[assembly: OwinStartupAttribute(typeof(NbuReservationSystem.Web.Startup))]

namespace NbuReservationSystem.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
