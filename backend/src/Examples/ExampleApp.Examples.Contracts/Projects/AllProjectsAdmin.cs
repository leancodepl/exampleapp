#if Example
using LeanCode.Contracts.Admin;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Projects;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class AllProjectsAdmin : AdminQuery<AdminProjectDTO>
{
    [AdminFilterFor(nameof(AdminProjectDTO.Name))]
    public string? NameFilter { get; set; }
}

public class AdminProjectDTO
{
    public string Id { get; set; }

    [AdminColumn("Name"), AdminSortable]
    public string Name { get; set; }
}
#endif
