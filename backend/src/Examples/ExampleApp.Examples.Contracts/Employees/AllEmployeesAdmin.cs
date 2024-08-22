using LeanCode.Contracts.Admin;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Employees;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class AllEmployeesAdmin : AdminQuery<AdminEmployeeDTO>
{
    [AdminFilterFor(nameof(AdminEmployeeDTO.Name))]
    public string? NameFilter { get; set; }
}

public class AdminEmployeeDTO
{
    public string Id { get; set; }

    [AdminColumn("Name"), AdminSortable]
    public string Name { get; set; }
}
