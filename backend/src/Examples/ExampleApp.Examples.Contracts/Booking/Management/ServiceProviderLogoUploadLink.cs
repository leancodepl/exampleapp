using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Booking.Management;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class ServiceProviderLogoUploadLink : IQuery<ServiceProviderLogoUploadLinkDTO> { }

public class ServiceProviderLogoUploadLinkDTO
{
    public Uri Link { get; set; }
    public Dictionary<string, string> RequiredHeaders { get; set; }
}
