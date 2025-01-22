using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Contracts.Booking.Management;
using ExampleApp.Examples.Contracts.Booking.Reservations;
using ExampleApp.Examples.Contracts.Booking.ServiceProviders;
using ExampleApp.Examples.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace ExampleApp.Examples.IntegrationTests.Booking;

public class ReservationTests_MultipleReservations : TestsBase<MultiUserExampleAppTestApp>
{
    [Fact]
    public async Task Double_reservation_of_the_same_timeslot()
    {
        var spId = await CreateServiceProviderAsync();
        var timeslot = await AddTimeslotAsync(spId);

        await App.Commands[0]
            .RunSuccessAsync(new ReserveTimeslot { TimeslotId = timeslot.Id, CalendarDayId = timeslot.CalendarDayId });
        await App.Commands[1]
            .RunSuccessAsync(new ReserveTimeslot { TimeslotId = timeslot.Id, CalendarDayId = timeslot.CalendarDayId });

        // This test relies on an event handler retry, thus the bus needs more time to stabilize.
        await App.WaitForBusAsync(delay: TimeSpan.FromSeconds(30));

        var details0 = await App.Queries[0]
            .GetAsync(
                new MyReservationByTimeslotId { TimeslotId = timeslot.Id },
                TestContext.Current.CancellationToken
            );
        var details1 = await App.Queries[1]
            .GetAsync(
                new MyReservationByTimeslotId { TimeslotId = timeslot.Id },
                TestContext.Current.CancellationToken
            );

        details0.Should().NotBeNull();
        details1.Should().NotBeNull();
        new[] { details0!.Status, details1!.Status }
            .Should()
            .BeEquivalentTo([ReservationStatusDTO.Confirmed, ReservationStatusDTO.Rejected]);
    }

    private async Task<TimeslotDTO> AddTimeslotAsync(string spId)
    {
        var date = new DateOnly(2024, 10, 23);
        var from = new TimeOnly(14, 0);
        var addTimeslot = new AddTimeslot
        {
            ServiceProviderId = spId,
            Date = date,
            StartTime = from,
            EndTime = new(15, 0),
            Price = new MoneyDTO((int)(10m * 100), "PLN"),
        };

        await App.Commands[0].RunSuccessAsync(addTimeslot);

        var details = await App.Queries[0]
            .GetAsync(new ServiceProviderDetails { ServiceProviderId = spId, CalendarDate = date });
        return details!.Timeslots.Should().ContainSingle(t => t.StartTime == from).Which;
    }

    private async Task<string> CreateServiceProviderAsync()
    {
        var fakeName = Guid.NewGuid().ToString();
        var createServiceProvider = new CreateServiceProvider
        {
            Name = fakeName,
            Type = ServiceProviderTypeDTO.Hairdresser,
            Description = "Description",
            CoverPhoto = new Uri("http://example.com"),
            Thumbnail = new Uri("http://example.com"),
            Address = "Address",
            Location = new LocationDTO(10, 10),
        };

        await App.Commands[0].RunSuccessAsync(createServiceProvider);
        var serviceProvider = await App.Queries[0].GetAsync(new AllServiceProviders { PageSize = 100 });
        return serviceProvider.Items.Should().ContainSingle(e => e.Name == fakeName).Which.Id;
    }
}
