using System.Text;
using ExampleApp.Examples.Configuration;
using ExampleApp.Examples.Contracts.Dashboards;
using LeanCode.CQRS.Execution;
using LeanCode.TimeProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ExampleApp.Examples.Handlers.Dashboards;

public class AssignmentEmployerEmbedQH : IQueryHandler<AssignmentEmployerEmbed, Uri>
{
    private readonly MetabaseConfiguration config;

    public AssignmentEmployerEmbedQH(IOptions<MetabaseConfiguration> config)
    {
        this.config = config.Value;
    }

    public Task<Uri> ExecuteAsync(HttpContext context, AssignmentEmployerEmbed query)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.MetabaseSecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var expiration = Time.UtcNow.AddMinutes(10);

        var tokenHandler = new JsonWebTokenHandler();
        var tokenString = tokenHandler.CreateToken(
            new SecurityTokenDescriptor
            {
                Claims = new Dictionary<string, object?>
                {
                    ["resource"] = new Dictionary<string, object?>
                    {
                        ["question"] = config.AssignmentEmployerEmbedQuestion,
                    },
                    ["params"] = new Dictionary<string, object?>(),
                },
                Expires = expiration,
                SigningCredentials = credentials,
            }
        );

        return Task.FromResult(new Uri($"{config.MetabaseUrl}/embed/question/{tokenString}#bordered=true&titled=true"));
    }
}
