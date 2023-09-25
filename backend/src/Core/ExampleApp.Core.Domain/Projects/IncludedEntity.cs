namespace ExampleApp.Core.Domain.Projects;

public class IncludedEntity
{
    public Project Project { get; private init; } = null!;
    public ProjectId ProjectId { get; private init; }
    public int SomeInt { get; private set; }
    public string SomeString { get; private set; } = null!;

    private IncludedEntity() { }

    public IncludedEntity(Project project, int someInt, string someString)
    {
        Project = project;
        ProjectId = project.Id;
        SomeInt = someInt;
        SomeString = someString;
    }

    public void Update(string someString)
    {
        SomeString = someString;
    }
}
