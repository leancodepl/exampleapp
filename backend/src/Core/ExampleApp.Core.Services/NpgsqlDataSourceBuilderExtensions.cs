using Azure.Core;
using Npgsql;

namespace ExampleApp.Core.Services;

public static class NpgsqlDataSourceBuilderExtensions
{
    public static NpgsqlDataSourceBuilder UseAzureActiveDirectoryAuthentication(
        this NpgsqlDataSourceBuilder builder,
        TokenCredential credential
    )
    {
        return builder.UsePeriodicPasswordProvider(
            async (b, ct) =>
            {
                var token = await credential.GetTokenAsync(
                    new(new[] { "https://ossrdbms-aad.database.windows.net" }, null),
                    ct
                );

                return token.Token;
            },
            TimeSpan.FromMinutes(5),
            TimeSpan.FromSeconds(1)
        );
    }
}
