using LeanCode.Components;
using LeanCode.DomainModels.MassTransitRelay.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ExampleApp.IntegrationTests.Overrides
{
    public class TestStartup : Api.Startup
    {
        public TestStartup(IWebHostEnvironment hostEnv, IConfiguration config)
            : base(hostEnv, config)
        {
            Modules = base.Modules
                .Prepend(new TestOverridesPreModule())
                .Append(new MassTransitTestRelayModule())
                .Append(new TestOverridesPostModule())
                .ToArray();
        }

        protected override IAppModule[] Modules { get; }
    }
}
