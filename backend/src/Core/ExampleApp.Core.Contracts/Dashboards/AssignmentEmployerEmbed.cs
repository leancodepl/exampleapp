using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Core.Contracts.Dashboards;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class AssignmentEmployerEmbed : IQuery<Uri> { }
