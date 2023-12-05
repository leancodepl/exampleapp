using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.Domain.Events;
using LeanCode.Pipe;
using MassTransit;

namespace ExampleApp.Examples.Services.Processes.Projects;

public class PublishEmployeeUnassignedFromAssignmentNotification : IConsumer<EmployeeUnassignedFromAssignment>
{
    private readonly ILeanPipePublisher<ProjectEmployeesAssignmentsTopic> topicPublisher;

    public PublishEmployeeUnassignedFromAssignmentNotification(
        ILeanPipePublisher<ProjectEmployeesAssignmentsTopic> topicPublisher
    )
    {
        this.topicPublisher = topicPublisher;
    }

    public Task Consume(ConsumeContext<EmployeeUnassignedFromAssignment> context)
    {
        var msg = context.Message;

        var topic = new ProjectEmployeesAssignmentsTopic { ProjectId = msg.ProjectId };
        var notification = new EmployeeUnassignedFromAssignmentDTO { AssignmentId = msg.AssignmentId, };

        return topicPublisher.PublishAsync(topic, notification, context.CancellationToken);
    }
}
