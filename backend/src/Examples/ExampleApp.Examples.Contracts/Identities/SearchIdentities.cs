using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Identities;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class SearchIdentities : SortedQuery<KratosIdentityDTO, KratosIdentitySortKeyDTO>
{
    public string? SchemaId { get; set; }
    public string? EmailPattern { get; set; }
    public string? GivenNamePattern { get; set; }
    public string? FamilyNamePattern { get; set; }
}

public enum KratosIdentitySortKeyDTO
{
    CreatedAt = 0,
    Email = 1,
    GivenName = 2,
    FamilyName = 3,
}

public class KratosIdentityDTO
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string SchemaId { get; set; }
    public string Email { get; set; }
    public string GivenName { get; set; }
    public string? FamilyName { get; set; }

    [ExcludeFromContractsGeneration]
    public object Traits { get; set; }

    [ExcludeFromContractsGeneration]
    public object? MetadataPublic { get; set; }

    [ExcludeFromContractsGeneration]
    public object? MetadataAdmin { get; set; }
}
