using ExampleApp.Examples.Contracts;
using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Contracts.Booking.ServiceProviders;
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Services.DataAccess;
using LeanCode.CQRS.Execution;
using LeanCode.QueryableExtensions;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Services.CQRS.Booking.ServiceProviders;

public class AllServiceProvidersQH : IQueryHandler<AllServiceProviders, PaginatedResult<ServiceProviderSummaryDTO>>
{
    private readonly ExamplesDbContext dbContext;

    public AllServiceProvidersQH(ExamplesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

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
                ListItemPicture = sp.ListItemPicture,
                IsPromotionActive = sp.IsPromotionActive,
                Address = sp.Address,
                Location = new(sp.Location.Latitude, sp.Location.Longitude),
            })
            .ToPaginatedResultAsync(query, context.RequestAborted);
    }

    private static IQueryable<ServiceProvider> ApplyFilters(AllServiceProviders query, IQueryable<ServiceProvider> q)
    {
        q = q.ConditionalWhere(sp => sp.Name.Contains(query.NameFilter!), !string.IsNullOrEmpty(query.NameFilter));
        q = q.ConditionalWhere(sp => sp.Type == (ServiceProviderType)query.TypeFilter!, query.TypeFilter != null);
        q = q.ConditionalWhere(sp => sp.IsPromotionActive, query.PromotedOnly);
        return q;
    }

    private static IQueryable<ServiceProvider> ApplySort(AllServiceProviders query, IQueryable<ServiceProvider> q)
    {
        return query.SortBy switch
        {
            ServiceProviderSortFieldsDTO.Name => q.OrderBy(sp => sp.Name, query.SortByDescending),
            ServiceProviderSortFieldsDTO.Type => q.OrderBy(sp => sp.Type, query.SortByDescending),
            _ => q.OrderBy(sp => sp.Id),
        };
    }
}
