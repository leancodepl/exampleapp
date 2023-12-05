using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Users;

[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
public class DeleteOwnAccount : ICommand { }
