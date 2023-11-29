using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.LeanPipeFunnel.Handlers;

public static class VersionHandler
{
    private static readonly string Version;

    public static Task HandleAsync(HttpContext ctx)
    {
        ctx.Response.StatusCode = 200;
        return ctx.Response.WriteAsync(Version);
    }

    static VersionHandler()
    {
        var self = Assembly.GetExecutingAssembly();
        var version = self.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "0.0.0";
        var name = self.GetName().Name;
        Version = $"{name} {version}";
    }
}
