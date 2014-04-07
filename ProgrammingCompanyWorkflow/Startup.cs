using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProgrammingCompanyWorkflow.Startup))]
namespace ProgrammingCompanyWorkflow
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
