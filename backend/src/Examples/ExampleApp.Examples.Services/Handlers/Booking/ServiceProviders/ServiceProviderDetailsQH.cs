using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Contracts.Booking.ServiceProviders;
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Services.DataAccess;
using LeanCode.CQRS.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.Handlers.Booking.ServiceProviders;

public class ServiceProviderDetailsQH(ExamplesDbContext dbContext)
    : IQueryHandler<ServiceProviderDetails, ServiceProviderDetailsDTO?>
{
    public async Task<ServiceProviderDetailsDTO?> ExecuteAsync(HttpContext context, ServiceProviderDetails query)
    {
        if (!ServiceProviderId.TryParse(query.ServiceProviderId, out var serviceProviderId))
        {
            return null;
        }

        var q =
            from sp in dbContext.ServiceProviders
            join cd in dbContext.CalendarDays
                on new { sp.Id, Date = query.CalendarDate } equals new { Id = cd.ServiceProviderId, cd.Date }
                into calendarDays
            from cd in calendarDays.DefaultIfEmpty()
            where sp.Id == serviceProviderId
            select new ServiceProviderDetailsDTO
            {
                Id = sp.Id,
                Name = sp.Name,
                Description = sp.Description,
                Type = (ServiceProviderTypeDTO)sp.Type,
                Address = sp.Address,
                Location = sp.Location.ToDTO(),
                IsPromotionActive = sp.IsPromotionActive,
                PromotionalBanner = sp.PromotionalBanner,
                ListItemPicture = sp.ListItemPicture,
                AvailableTimeslots = cd
                    .Timeslots.OrderBy(ts => ts.StartTime)
                    .Select(ts => new AvailableTimeslotDTO
                    {
                        Id = ts.Id,
                        CalendarDayId = cd.Id,
                        StartTime = ts.StartTime,
                        EndTime = ts.EndTime,
                        Price = ts.Price.ToDTO(),
                    })
                    .ToList(),
            };
        return await q.FirstOrDefaultAsync(context.RequestAborted);
    }
}
