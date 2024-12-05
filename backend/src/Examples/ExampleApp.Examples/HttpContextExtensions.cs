using System.Security.Claims;
using ExampleApp.Examples.Contracts;
using Microsoft.AspNetCore.Http;
#if Example
using ExampleApp.Examples.Domain.Booking;
#endif

namespace ExampleApp.Examples;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext context)
    {
        var claim = context.User.FindFirstValue(Auth.KnownClaims.UserId);

        ArgumentException.ThrowIfNullOrEmpty(claim);

        return Guid.Parse(claim);
    }

    public static bool TryGetUserId(this HttpContext context, out Guid userId)
    {
        var claim = context.User.FindFirstValue(Auth.KnownClaims.UserId);
        return Guid.TryParse(claim, out userId);
    }

#if Example
    public static CustomerId GetCustomerId(this HttpContext context)
    {
        return CustomerId.Parse(context.GetUserId());
    }
#endif
}
