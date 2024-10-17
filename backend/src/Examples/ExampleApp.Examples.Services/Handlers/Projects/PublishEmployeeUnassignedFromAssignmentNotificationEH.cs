using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.Domain.Projects.Events;
using LeanCode.Pipe;
using MassTransit;

namespace ExampleApp.Examples.Services.Handlers.Projects;

public class PublishEmployeeUnassignedFromAssignmentNotificationEH : IConsumer<EmployeeUnassignedFromAssignment>
{
    private readonly ILeanPipePublisher<ProjectEmployeesAssignmentsTopic> topicPublisher;

    public PublishEmployeeUnassignedFromAssignmentNotificationEH(
        ILeanPipePublisher<ProjectEmployeesAssignmentsTopic> topicPublisher
    )
    {
        this.topicPublisher = topicPublisher;
    }

    public Task Consume(ConsumeContext<EmployeeUnassignedFromAssignment> context)
    {
        var msg = context.Message;

        var topic = new ProjectEmployeesAssignmentsTopic { ProjectId = msg.ProjectId };
        var notification = new EmployeeUnassignedFromAssignmentDTO { AssignmentId = msg.AssignmentId };

        return topicPublisher.PublishAsync(topic, notification, context.CancellationToken);
    }
}
