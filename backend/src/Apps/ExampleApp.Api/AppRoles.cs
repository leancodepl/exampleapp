using LeanCode.CQRS.Security;
using R = ExampleApp.Examples.Contracts.Auth.Roles;

namespace ExampleApp.Api;

internal class AppRoles : IRoleRegistration
{
    public IEnumerable<Role> Roles { get; } =
        [new Role(R.User, R.User, LeanCode.AppRating.Contracts.RatingPermissions.RateApp), new Role(R.Admin, R.Admin)];
}
