using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Contracts.Booking.ServiceProviders;
using FluentAssertions;
using Xunit;

namespace ExampleApp.Examples.IntegrationTests.Booking;

public class ServiceProviderIntegrationTests : BookingTestsBase
{
    [Fact]
    public async Task Creating_and_listing_ServiceProviders()
    {
        var sp1Id = await CreateServiceProviderAsync("ServiceProvider 1", ServiceProviderTypeDTO.Hairdresser, 2);
        var sp2Id = await CreateServiceProviderAsync("ServiceProvider 2", ServiceProviderTypeDTO.Groomer, 5);
        var sp3Id = await CreateServiceProviderAsync("ServiceProvider 3", ServiceProviderTypeDTO.Hairdresser, 3);

        var serviceProviders = await App.Query.GetAsync(new AllServiceProviders { PageSize = 100 });
        serviceProviders
            .Items.Should()
            .BeEquivalentTo(
                [
                    new { Name = "ServiceProvider 1" },
                    new { Name = "ServiceProvider 2" },
                    new { Name = "ServiceProvider 3" },
                ]
            );

        var sorted = await App.Query.GetAsync(
            new AllServiceProviders { SortBy = ServiceProviderSortFieldsDTO.Type, PageSize = 100 }
        );
        sorted
            .Items.Should()
            .BeEquivalentTo(
                [new { Id = sp1Id }, new { Id = sp3Id }, new { Id = sp2Id }],
                opts => opts.WithStrictOrdering()
            );

        var sortedByRatings = await App.Query.GetAsync(
            new AllServiceProviders
            {
                SortBy = ServiceProviderSortFieldsDTO.Ratings,
                PageSize = 100,
                SortByDescending = true,
            }
        );
        sortedByRatings
            .Items.Should()
            .BeEquivalentTo(
                [new { Id = sp2Id }, new { Id = sp3Id }, new { Id = sp1Id }],
                opts => opts.WithStrictOrdering()
            );

        var filteredServiceProviders = await App.Query.GetAsync(
            new AllServiceProviders { NameFilter = "2", PageSize = 100 }
        );
        filteredServiceProviders.Items.Should().BeEquivalentTo([new { Id = sp2Id }]);

        var sp1Details = await App.Query.GetAsync(new ServiceProviderDetails { ServiceProviderId = sp1Id });
        sp1Details
            .Should()
            .BeEquivalentTo(
                new
                {
                    Id = sp1Id,
                    Name = "ServiceProvider 1",
                    Type = ServiceProviderTypeDTO.Hairdresser,
                }
            );

        var sp2Details = await App.Query.GetAsync(new ServiceProviderDetails { ServiceProviderId = sp2Id });
        sp2Details
            .Should()
            .BeEquivalentTo(
                new
                {
                    Id = sp2Id,
                    Name = "ServiceProvider 2",
                    Type = ServiceProviderTypeDTO.Groomer,
                }
            );
    }
}
