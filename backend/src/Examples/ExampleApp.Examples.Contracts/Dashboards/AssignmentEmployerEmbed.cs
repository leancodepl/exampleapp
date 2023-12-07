#if Example
using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Dashboards;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class AssignmentEmployerEmbed : IQuery<Uri> { }
#endif
