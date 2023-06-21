using System.Text.Json;
using System.Text.Json.Serialization;
using MassTransit;
using Microsoft.AspNetCore.Http;
using LeanCode.Kratos.Client.Model;
using ExampleApp.Core.Services.Processes.Kratos;

namespace ExampleApp.Api.Handlers;

public partial class KratosIdentitySyncHandler : KratosWebHookHandler
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<KratosIdentitySyncHandler>();

    private readonly IBus bus;

    public KratosIdentitySyncHandler(Config config, IBus bus)
        : base(config)
    {
        this.bus = bus;
    }

    protected override async Task HandleCoreAsync(HttpContext ctx)
    {
        var body = await JsonSerializer.DeserializeAsync(
            ctx.Request.Body,
            KratosIdentitySyncHandlerContext.Default.RequestBody,
            ctx.RequestAborted
        );

        var identity = body.Identity;

        if (identity is null)
        {
            logger.Error("Identity is null");
            ctx.Response.StatusCode = 422;
            await ctx.Response.WriteAsJsonAsync(
                new(null, new(1) { new(null, new(1) { new(1, "identity is null", "error") }) }),
                KratosWebHookHandlerContext.Default.ResponseBody,
                cancellationToken: ctx.RequestAborted
            );
            return;
        }
        else if (identity.Id == default)
        {
            logger.Error("Identity Id is empty");
            ctx.Response.StatusCode = 422;
            await ctx.Response.WriteAsJsonAsync(
                new(null, new(1) { new(null, new(1) { new(1, "identity.id is empty", "error") }) }),
                KratosWebHookHandlerContext.Default.ResponseBody,
                cancellationToken: ctx.RequestAborted
            );
            return;
        }

        await bus.Publish(
            new KratosIdentityUpdated(Guid.NewGuid(), LeanCode.Time.TimeProvider.Now, identity),
            ctx.RequestAborted
        );
        ctx.Response.StatusCode = 200;

        logger.Information("Successfully processed sync webhook for identity {IdentityId}", identity.Id);
    }

    public record struct RequestBody([property: JsonPropertyName("identity")] Identity? Identity);

#if NET8_0_OR_GREATER
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
#endif
    [JsonSerializable(typeof(RequestBody))]
    private partial class KratosIdentitySyncHandlerContext : JsonSerializerContext { }
}
