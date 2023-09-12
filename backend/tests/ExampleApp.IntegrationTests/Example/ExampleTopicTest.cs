using ExampleApp.Core.Contracts.Employees;
using ExampleApp.Core.Contracts.Projects;
using FluentAssertions;
using LeanCode.Contracts;
using LeanPipe.TestClient;
using Xunit;

namespace ExampleApp.IntegrationTests.Example;

public class ExampleTopicTest : TestsBase<AuthenticatedExampleAppTestApp>
{
    [Fact]
    public async Task Example_topic_test()
    {
        await App.Command.RunAsync(new CreateProject { Name = "Project" });

        var projects = await App.Query.GetAsync(new AllProjects());
        var projectSummary = projects.Should().ContainSingle().Subject;

        await App.Command.RunAsync(
            new AddAssignmentsToProject
            {
                ProjectId = projectSummary.Id,
                Assignments = new() { new() { Name = "Assignment" } },
            }
        );

        var project = await App.Query.GetAsync(new ProjectDetails { Id = projectSummary.Id });
        var assignment = project.Assignments.Should().ContainSingle().Subject;

        await App.Command.RunAsync(new CreateEmployee { Name = "Employee", Email = "employee@leancode.pl" });

        var employees = await App.Query.GetAsync(new AllEmployees());
        var employee = employees.Should().ContainSingle().Subject;

        var projectEmployeesAssignmentsTopic = new ProjectEmployeesAssignmentsTopic { ProjectId = project.Id, };

        await App.LeanPipe.SubscribeSuccessAsync(projectEmployeesAssignmentsTopic);

        await App.Command.RunAsync(
            new AssignEmployeeToAssignment { AssignmentId = assignment.Id, EmployeeId = employee.Id }
        );
        await App.WaitForBusAsync();

        App.LeanPipe
            .NotificationsOn(projectEmployeesAssignmentsTopic)
            .Should()
            .ContainSingle()
            .Which.Should()
            .BeEquivalentTo(
                new EmployeeAssignedToAssignmentDTO { AssignmentId = assignment.Id, EmployeeId = employee.Id },
                opts => opts.RespectingRuntimeTypes()
            );

        await App.Command.RunAsync(new UnassignEmployeeFromAssignment { AssignmentId = assignment.Id });
        await App.WaitForBusAsync();

        App.LeanPipe
            .NotificationsOn(projectEmployeesAssignmentsTopic)
            .Should()
            .HaveCount(2)
            .And.Subject.Last()
            .Should()
            .BeEquivalentTo(
                new EmployeeUnassignedFromAssignmentDTO { AssignmentId = assignment.Id },
                opts => opts.RespectingRuntimeTypes()
            );

        await App.LeanPipe.UnsubscribeSuccessAsync(projectEmployeesAssignmentsTopic);
    }
}
