using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Core.Contracts.Users;

[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
public class DeleteOwnAccount : ICommand { }
