using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.IntegrationTests.Helpers;
using Xunit;

namespace ExampleApp.Examples.IntegrationTests.Example;

public class ExampleTest : TestsBase<UnauthenticatedExampleAppTestApp>
{
    [Fact]
    public async Task Example_test()
    {
        await App.Command.RunSuccessAsync(new CreateProject { Name = "Project" });

        var projects = await App.Query.GetAsync(new AllProjects(), TestContext.Current.CancellationToken);
        var project = Assert.Single(projects);

        Assert.Equal("Project", project.Name);
        Assert.Matches("^project_[0-7][0-9A-HJKMNP-TV-Z]{25}$", project.Id);

        var projectDetails = await App.Query.GetAsync(
            new ProjectDetails { Id = project.Id },
            TestContext.Current.CancellationToken
        );

        Assert.NotNull(projectDetails);
        Assert.Equal(project.Id, projectDetails.Id);
        Assert.Equal(project.Name, projectDetails.Name);
        Assert.Empty(projectDetails.Assignments);
    }
}
