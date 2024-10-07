using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Contracts.Booking.ServiceProviders;
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Services.DataAccess;
using LeanCode.CQRS.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.CQRS.Booking.ServiceProviders;

public class ServiceProviderDetailsQH(ExamplesDbContext dbContext)
    : IQueryHandler<ServiceProviderDetails, ServiceProviderDetailsDTO?>
{
    public async Task<ServiceProviderDetailsDTO?> ExecuteAsync(HttpContext context, ServiceProviderDetails query)
    {
        if (!ServiceProviderId.TryParse(query.ServiceProviderId, out var serviceProviderId))
        {
            return null;
        }

        return await dbContext
            .ServiceProviders.Where(sp => sp.Id == serviceProviderId)
            .Join(
                dbContext.CalendarDays,
                sp => new { sp.Id, Date = query.CalendarDate },
                cd => new { Id = cd.ServiceProviderId, cd.Date },
                (sp, cd) =>
                    new ServiceProviderDetailsDTO
                    {
                        Id = sp.Id,
                        Name = sp.Name,
                        Description = sp.Description,
                        Type = (ServiceProviderTypeDTO)sp.Type,
                        Address = sp.Address,
                        Location = new(sp.Location.Latitude, sp.Location.Longitude),
                        IsPromotionActive = sp.IsPromotionActive,
                        PromotionalBanner = sp.PromotionalBanner,
                        ListItemPicture = sp.ListItemPicture,
                        AvailableTimeslots = cd
                            .Timeslots.Select(ts => new AvailableTimeslotDTO
                            {
                                Id = ts.Id,
                                StartTime = ts.StartTime,
                                EndTime = ts.EndTime,
                                Price = new((int)(ts.Price.Value * 100), ts.Price.Currency),
                            })
                            .ToList(),
                    }
            )
            .FirstOrDefaultAsync(context.RequestAborted);
    }
}
