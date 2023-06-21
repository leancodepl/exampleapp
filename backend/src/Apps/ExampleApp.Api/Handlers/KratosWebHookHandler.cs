using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using LeanCode.Kratos.Client.Model;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Api.Handlers;

public abstract partial class KratosWebHookHandler
{
    public sealed record class Config(string ApiKey);

    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<KratosWebHookHandler>();

    private readonly Config config;

    public KratosWebHookHandler(Config config)
    {
        this.config = config;
    }

    protected virtual string ApiKeyHeaderName => "X-Api-Key";

    protected abstract Task HandleCoreAsync(HttpContext ctx);

    public async Task HandleAsync(HttpContext ctx)
    {
        try
        {
            if (VerifyApiKey(ctx.Request.Headers[ApiKeyHeaderName].ToString(), config.ApiKey))
            {
                await HandleCoreAsync(ctx);
            }
            else
            {
                logger.Error("Invalid Api Key");
                ctx.Response.StatusCode = 403;
            }
        }
        catch (Exception e)
        {
            logger.Error(e, "Failed to process webhook");
            ctx.Response.StatusCode = 500;
        }
    }

    private static bool VerifyApiKey(string left, string right)
    {
        var leftBytes = MemoryMarshal.AsBytes(left.AsSpan());
        var rightBytes = MemoryMarshal.AsBytes(right.AsSpan());

        return CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }

    public record struct ResponseBody(
        [property: JsonPropertyName("messages")] Identity? Identity,
        [property: JsonPropertyName("messages")] List<ErrorMessage>? Messages
    );

    public record struct ErrorMessage(
        [property: JsonPropertyName("instance_ptr")] string? InstancePtr,
        [property: JsonPropertyName("messages")] List<DetailedMessage> DetailedMessages
    );

    public record struct DetailedMessage(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("text")] string Text,
        [property: JsonPropertyName("type")] string Type
    );

    [JsonSerializable(typeof(ResponseBody))]
    public partial class KratosWebHookHandlerContext : JsonSerializerContext { }
}
