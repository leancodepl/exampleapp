using ExampleApp.Core.Contracts.Employees;
using ExampleApp.Core.Contracts.Projects;
using ExampleApp.IntegrationTests.Helpers;
using FluentAssertions;
using LeanCode.Pipe.TestClient;
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
            var nextNotification = App.LeanPipe.WaitForNextNotificationOn(topic);
            await AssignEmployeeToAssignmentAsync(assignment, employee);
            var notification = await nextNotification;

            App.LeanPipe
                .NotificationsOn(topic)
                .Should()
                .ContainSingle()
                .Which.Should()
                .BeSameAs(notification)
                .And.BeEquivalentTo(
                    new EmployeeAssignedToAssignmentDTO { AssignmentId = assignment.Id, EmployeeId = employee.Id },
                    opts => opts.RespectingRuntimeTypes()
                );
        }

        async Task CheckNotificationOnUnassigningEmployeeFromAssignment()
        {
            var nextNotification = App.LeanPipe.WaitForNextNotificationOn(topic);
            await UnassignEmployeeFromAssignmentAsync(assignment);
            var notification = await nextNotification;

            App.LeanPipe
                .NotificationsOn(topic)
                .Should()
                .HaveCount(2)
                .And.Subject.First()
                .Should()
                .BeSameAs(notification)
                .And.BeEquivalentTo(
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
        await App.Command.RunSuccessAsync(new UnassignEmployeeFromAssignment { AssignmentId = assignment.Id });
    }

    private async Task AssignEmployeeToAssignmentAsync(AssignmentDTO assignment, EmployeeDTO employee)
    {
        await App.Command.RunSuccessAsync(
            new AssignEmployeeToAssignment { AssignmentId = assignment.Id, EmployeeId = employee.Id }
        );
    }

    private async Task<EmployeeDTO> CreateEmployeeAsync()
    {
        await App.Command.RunSuccessAsync(new CreateEmployee { Name = "Employee", Email = "employee@leancode.pl" });

        var employees = await App.Query.GetAsync(new AllEmployees());
        return employees.Should().ContainSingle().Subject;
    }

    private async Task<AssignmentDTO> AddAssignmentToProjectAsync(ProjectDTO projectSummary)
    {
        await App.Command.RunSuccessAsync(
            new AddAssignmentsToProject
            {
                ProjectId = projectSummary.Id,
                Assignments = new() { new() { Name = "Assignment" } },
            }
        );

        var project = await App.Query.GetAsync(new ProjectDetails { Id = projectSummary.Id });
        return project.Assignments.Should().ContainSingle().Subject;
    }

    private async Task<ProjectDTO> CreateProjectAsync()
    {
        await App.Command.RunSuccessAsync(new CreateProject { Name = "Project" });

        var projects = await App.Query.GetAsync(new AllProjects());
        return projects.Should().ContainSingle().Subject;
    }
}
