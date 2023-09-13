using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Core.Contracts.Employees;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class AllEmployees : IQuery<List<EmployeeDTO>> { }

public class EmployeeDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
