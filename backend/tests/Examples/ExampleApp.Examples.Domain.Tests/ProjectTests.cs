using ExampleApp.Examples.Domain.Projects;
using Xunit;

namespace ExampleApp.Examples.Domain.Tests;

public class ProjectTests
{
    [Fact]
    public void New_project_has_no_assignments()
    {
        var project = Project.Create("test");

        Assert.Empty(project.Assignments);
    }
}
