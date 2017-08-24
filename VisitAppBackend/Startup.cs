using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.Extensions.DependencyInjection;
using VisitAppBackend.Clients;

[assembly: OwinStartup(typeof(VisitAppBackend.Startup))]

namespace VisitAppBackend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
