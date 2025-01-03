using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Booking.ServiceProviders;

[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
public class AllServiceProviders : SortedQuery<ServiceProviderSummaryDTO, ServiceProviderSortFieldsDTO>
{
    public string? NameFilter { get; set; }
    public ServiceProviderTypeDTO? TypeFilter { get; set; }
    public bool PromotedOnly { get; set; }
}

public enum ServiceProviderSortFieldsDTO
{
    Name = 0,
    Type = 1,
    Ratings = 2,
}

public class ServiceProviderSummaryDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public ServiceProviderTypeDTO Type { get; set; }
    public Uri Thumbnail { get; set; }
    public bool IsPromotionActive { get; set; }
    public string Address { get; set; }
    public LocationDTO Location { get; set; }
    public double Ratings { get; set; }
}
