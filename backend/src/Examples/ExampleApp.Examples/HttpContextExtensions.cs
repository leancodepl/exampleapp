using System.Security.Claims;
using ExampleApp.Examples.Contracts;
using ExampleApp.Examples.Domain.Booking;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext context)
    {
        var claim = context.User.FindFirstValue(Auth.KnownClaims.UserId);

        ArgumentException.ThrowIfNullOrEmpty(claim);

        return Guid.Parse(claim);
    }

    public static CustomerId GetCustomerId(this HttpContext context)
    {
        return CustomerId.Parse(context.GetUserId());
    }
}
