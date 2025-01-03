using ExampleApp.Examples.Contracts;
using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Contracts.Booking.ServiceProviders;
using ExampleApp.Examples.DataAccess;
using ExampleApp.Examples.Domain.Booking;
using LeanCode.CQRS.Execution;
using LeanCode.QueryableExtensions;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Handlers.Booking.ServiceProviders;

public class AllServiceProvidersQH(ExamplesDbContext dbContext)
    : IQueryHandler<AllServiceProviders, PaginatedResult<ServiceProviderSummaryDTO>>
{
    public async Task<PaginatedResult<ServiceProviderSummaryDTO>> ExecuteAsync(
        HttpContext context,
        AllServiceProviders query
    )
    {
        var serviceProviders = ApplyFilters(query, dbContext.ServiceProviders);
        serviceProviders = ApplySort(query, serviceProviders);

        return await serviceProviders
            .Select(sp => new ServiceProviderSummaryDTO
            {
                Id = sp.Id,
                Name = sp.Name,
                Type = (ServiceProviderTypeDTO)sp.Type,
                Thumbnail = sp.Thumbnail,
                IsPromotionActive = sp.IsPromotionActive,
                Address = sp.Address,
                Location = new(sp.Location.Latitude, sp.Location.Longitude),
                Ratings = sp.Ratings,
            })
            .ToPaginatedResultAsync(query, context.RequestAborted);
    }

    private static IQueryable<ServiceProvider> ApplyFilters(AllServiceProviders query, IQueryable<ServiceProvider> q)
    {
        return q.ConditionalWhere(sp => sp.Name.Contains(query.NameFilter!), !string.IsNullOrEmpty(query.NameFilter))
            .ConditionalWhere(sp => sp.Type == (ServiceProviderType)query.TypeFilter!, query.TypeFilter != null)
            .ConditionalWhere(sp => sp.IsPromotionActive, query.PromotedOnly);
    }

    private static IQueryable<ServiceProvider> ApplySort(AllServiceProviders query, IQueryable<ServiceProvider> q)
    {
        return query.SortBy switch
        {
            ServiceProviderSortFieldsDTO.Name => q.OrderBy(sp => sp.Name, query.SortByDescending),
            ServiceProviderSortFieldsDTO.Type => q.OrderBy(sp => sp.Type, query.SortByDescending).ThenBy(sp => sp.Name),
            ServiceProviderSortFieldsDTO.Ratings => q.OrderBy(sp => sp.Ratings, query.SortByDescending)
                .ThenBy(sp => sp.Name),
            _ => q.OrderBy(sp => sp.Id),
        };
    }
}
