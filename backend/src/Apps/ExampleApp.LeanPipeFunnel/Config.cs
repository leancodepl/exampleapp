namespace ExampleApp.LeanPipeFunnel;

public static class Config
{
    public static class Kratos
    {
        public static string PublicEndpoint(IConfiguration cfg) => cfg.GetString("Kratos:PublicEndpoint");
    }

    public static class MassTransit
    {
        public static class AzureServiceBus
        {
            public static string Endpoint(IConfiguration cfg) => cfg.GetString("MassTransit:AzureServiceBus:Endpoint");
        }

        public static class RabbitMq
        {
            public static Uri? Url(IConfiguration cfg) =>
                cfg.GetString("MassTransit:RabbitMq:Url") is string url ? new(url) : null;
        }
    }

    public static class Telemetry
    {
        public static string OtlpEndpoint(IConfiguration cfg) => cfg.GetString("Telemetry:Otlp:Endpoint");
    }

    public static class LeanPipe
    {
        public static bool EnableLeanPipeFunnel(IConfiguration cfg) => cfg.GetBool("LeanPipe:EnableLeanPipeFunnel");
    }

    private static string GetString(this IConfiguration configuration, string key)
    {
        return configuration.GetValue<string>(key)!;
    }

    private static bool GetBool(this IConfiguration configuration, string key)
    {
        return configuration.GetValue<bool>(key);
    }
}
