using LeanCode.CQRS.Security;
using R = ExampleApp.Examples.Contracts.Auth.Roles;

namespace ExampleApp.Examples;

internal class AppRoles : IRoleRegistration
{
    public IEnumerable<Role> Roles { get; } =
        [
            new Role(
                R.User,
                R.User
#if Example
                ,
                LeanCode.AppRating.Contracts.RatingPermissions.RateApp,
                LeanCode.NotificationCenter.Contracts.Permissions.NotificationCenter
#endif
            ),
            new Role(R.Admin, R.Admin),
        ];
}
