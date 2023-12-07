#if Example
using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Projects;

[AllowUnauthorized]
public class ProjectDetails : IQuery<ProjectDetailsDTO?>
{
    public string Id { get; set; }
}

public class ProjectDetailsDTO : ProjectDTO
{
    public List<AssignmentDTO> Assignments { get; set; }
}

public class AssignmentDTO : AssignmentWriteDTO
{
    public string Id { get; set; }
}
#endif
