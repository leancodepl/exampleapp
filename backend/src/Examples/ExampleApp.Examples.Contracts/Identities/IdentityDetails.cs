using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Identities;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class IdentityDetails : IQuery<KratosIdentityDetailsDTO?>
{
    public Guid Id { get; set; }
}

public class KratosIdentityDetailsDTO : KratosIdentityDTO
{
    public Dictionary<string, KratosIdentityCredentialDTO> Credentials { get; set; }
    public List<KratosIdentityRecoveryAddressDTO> RecoveryAddresses { get; set; }
    public List<KratosIdentityVerifiableAddressDTO> VerifiableAddresses { get; set; }
}

public enum KratosIdentityCredentialTypeDTO
{
    Password = 0,
    Oidc = 1,
    Totp = 2,
    LookupSecret = 3,
    Webauthn = 4,
    Code = 5,
    Passkey = 6,
}

public class KratosIdentityCredentialDTO
{
    public KratosIdentityCredentialTypeDTO Type { get; set; }
    public List<string> Identifiers { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class KratosIdentityRecoveryAddressDTO
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string Via { get; set; }
    public string Value { get; set; }
}

public class KratosIdentityVerifiableAddressDTO
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string Via { get; set; }
    public string Value { get; set; }
    public DateTimeOffset? VerifiedAt { get; set; }
}
