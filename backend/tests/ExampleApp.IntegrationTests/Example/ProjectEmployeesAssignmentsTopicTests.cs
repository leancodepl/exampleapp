using ExampleApp.Core.Contracts.Employees;
using ExampleApp.Core.Contracts.Projects;
using FluentAssertions;
using LeanPipe.TestClient;
using Xunit;

namespace ExampleApp.IntegrationTests.Example;

public class ProjectEmployeesAssignmentsTopicTests : TestsBase<AuthenticatedExampleAppTestApp>
{
    [Fact]
    public async Task Correct_notifications_are_published_to_ProjectEmployeesAssignmentsTopic()
    {
        var project = await CreateProjectAsync();
        var assignment = await AddAssignmentToProjectAsync(project);
        var employee = await CreateEmployeeAsync();

        var topic = await SubscribeToProjectEmployeesAssignmentsTopic(project);

        await CheckNotificationOnAssigningEmployeeToAssignment();
        await CheckNotificationOnUnassigningEmployeeFromAssignment();

        await UnsubscribeFromProjectEmployeesAssignmentsTopic(topic);

        async Task CheckNotificationOnAssigningEmployeeToAssignment()
        {
            await AssignEmployeeToAssignmentAsync(assignment, employee);

            App.LeanPipe
                .NotificationsOn(topic)
                .Should()
                .ContainSingle()
                .Which.Should()
                .BeEquivalentTo(
                    new EmployeeAssignedToAssignmentDTO { AssignmentId = assignment.Id, EmployeeId = employee.Id },
                    opts => opts.RespectingRuntimeTypes()
                );
        }

        async Task CheckNotificationOnUnassigningEmployeeFromAssignment()
        {
            await UnassignEmployeeFromAssignmentAsync(assignment);

            App.LeanPipe
                .NotificationsOn(topic)
                .Should()
                .HaveCount(2)
                .And.Subject.Last()
                .Should()
                .BeEquivalentTo(
                    new EmployeeUnassignedFromAssignmentDTO { AssignmentId = assignment.Id },
                    opts => opts.RespectingRuntimeTypes()
                );
        }
    }

    private async Task UnsubscribeFromProjectEmployeesAssignmentsTopic(ProjectEmployeesAssignmentsTopic topic)
    {
        await App.LeanPipe.UnsubscribeSuccessAsync(topic);
    }

    private async Task<ProjectEmployeesAssignmentsTopic> SubscribeToProjectEmployeesAssignmentsTopic(ProjectDTO project)
    {
        var projectEmployeesAssignmentsTopic = new ProjectEmployeesAssignmentsTopic { ProjectId = project.Id };

        await App.LeanPipe.SubscribeSuccessAsync(projectEmployeesAssignmentsTopic);
        return projectEmployeesAssignmentsTopic;
    }

    private async Task UnassignEmployeeFromAssignmentAsync(AssignmentDTO assignment)
    {
        var result = await App.Command.RunAsync(new UnassignEmployeeFromAssignment { AssignmentId = assignment.Id });
        result.WasSuccessful.Should().BeTrue();

        await App.WaitForBusAsync();
    }

    private async Task AssignEmployeeToAssignmentAsync(AssignmentDTO assignment, EmployeeDTO employee)
    {
        var result = await App.Command.RunAsync(
            new AssignEmployeeToAssignment { AssignmentId = assignment.Id, EmployeeId = employee.Id }
        );
        result.WasSuccessful.Should().BeTrue();

        await App.WaitForBusAsync();
    }

    private async Task<EmployeeDTO> CreateEmployeeAsync()
    {
        var result = await App.Command.RunAsync(
            new CreateEmployee { Name = "Employee", Email = "employee@leancode.pl" }
        );
        result.WasSuccessful.Should().BeTrue();

        var employees = await App.Query.GetAsync(new AllEmployees());
        var employee = employees.Should().ContainSingle().Subject;
        return employee;
    }

    private async Task<AssignmentDTO> AddAssignmentToProjectAsync(ProjectDTO projectSummary)
    {
        var result = await App.Command.RunAsync(
            new AddAssignmentsToProject
            {
                ProjectId = projectSummary.Id,
                Assignments = new() { new() { Name = "Assignment" } },
            }
        );
        result.WasSuccessful.Should().BeTrue();

        var project = await App.Query.GetAsync(new ProjectDetails { Id = projectSummary.Id });
        return project.Assignments.Should().ContainSingle().Subject;
    }

    private async Task<ProjectDTO> CreateProjectAsync()
    {
        var result = await App.Command.RunAsync(new CreateProject { Name = "Project" });
        result.WasSuccessful.Should().BeTrue();

        var projects = await App.Query.GetAsync(new AllProjects());
        return projects.Should().ContainSingle().Subject;
    }
}
