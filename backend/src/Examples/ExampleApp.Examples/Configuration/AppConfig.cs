using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;
#if Example
using LeanCode.Firebase;
using LeanCode.AppRating.Configuration;
#endif

namespace ExampleApp.Examples.Configuration;

public static class AppConfig
{
    public static class Kratos
    {
        public static string PublicEndpoint(IConfiguration cfg) => cfg.GetString("Kratos:PublicEndpoint")!;

        public static string AdminEndpoint(IConfiguration cfg) => cfg.GetString("Kratos:AdminEndpoint")!;

        public static string WebhookApiKey(IConfiguration cfg) => cfg.GetString("Kratos:WebhookApiKey")!;
    }

    public static class PostgreSQL
    {
        public static string ConnectionString(IConfiguration cfg) => cfg.GetString("PostgreSQL:ConnectionString")!;
    }

    public static class BlobStorage
    {
        public static string ConnectionString(IConfiguration cfg) => cfg.GetString("BlobStorage:ConnectionString")!;
    }

    public static class MassTransit
    {
        public static class AzureServiceBus
        {
            public static string Endpoint(IConfiguration cfg) => cfg.GetString("MassTransit:AzureServiceBus:Endpoint")!;
        }

        public static class RabbitMq
        {
            public static Uri? Url(IConfiguration cfg) =>
                cfg.GetString("MassTransit:RabbitMq:Url") is string url ? new(url) : null;
        }
    }

    public static class LeanPipe
    {
        public static bool EnableLeanPipeFunnel(IConfiguration cfg) => cfg.GetValue<bool>("LeanPipe:EnableFunnel");
    }

#if Example
    public static class Google
    {
        public static string? ApiKey(IConfiguration cfg) => cfg.GetString("Google:ApiKey");
    }
#endif

    public static class SendGrid
    {
        public static string? ApiKey(IConfiguration cfg) => cfg.GetString("SendGrid:ApiKey");
    }

    public static class CORS
    {
        public static string[] AllowedOrigins(IConfiguration cfg) =>
            cfg.GetSection("CORS:AllowedOrigins").Get<string[]>() ?? [];
    }

    public static class Logging
    {
        public static bool EnableDetailedInternalLogs(IConfiguration cfg) =>
            cfg.GetBool("Logging:EnableDetailedInternalLogs");

        public static LogEventLevel MinimumLevel(IConfiguration cfg) =>
            cfg.GetValue("Logging:MinimumLevel", LogEventLevel.Verbose);
    }

    public static class Telemetry
    {
        public static string? OtlpEndpoint(IConfiguration cfg) => cfg.GetString("Telemetry:Otlp:Endpoint");
    }

    public static class AuditLogs
    {
        public static string ContainerName(IConfiguration cfg) => cfg.GetString("AuditLogs:ContainerName")!;

        public static string TableName(IConfiguration cfg) => cfg.GetString("AuditLogs:TableName")!;
    }

#if Example
    public static class AppRating
    {
        public static string[] EmailsTo(IConfiguration cfg) =>
            cfg.GetSection("AppRating:EmailsTo").Get<string[]>() ?? Array.Empty<string>();
    }
#endif

    private static string? GetString(this IConfiguration configuration, string key)
    {
        return configuration.GetValue<string>(key);
    }

    private static bool GetBool(this IConfiguration configuration, string key)
    {
        return configuration.GetValue<bool>(key);
    }

    public static void AddMappedConfiguration(
        this IServiceCollection services,
        IConfiguration config,
        IWebHostEnvironment hostEnv
    )
    {
        services.AddSingleton(new LeanCode.Kratos.KratosWebHookHandlerConfig(Kratos.WebhookApiKey(config)));
#if Example
        services.AddSingleton(FirebaseConfiguration.Prepare(Google.ApiKey(config), Guid.NewGuid().ToString()));
        services.AddSingleton(
            new AppRatingReportsConfiguration(
                2.0,
                "en",
                "emails.low-rate-submitted.subject",
                "test+from@leancode.pl",
                AppRating.EmailsTo(config)
            )
        );
        services.Configure<MetabaseConfiguration>(config.GetSection("Metabase"));
#endif

        services.Configure<LeanCode.ConfigCat.ConfigCatOptions>(config.GetSection("ConfigCat"));
    }
}
