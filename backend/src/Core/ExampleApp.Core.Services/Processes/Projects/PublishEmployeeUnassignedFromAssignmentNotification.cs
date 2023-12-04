using ExampleApp.Core.Contracts.Projects;
using ExampleApp.Core.Domain.Events;
using LeanCode.Pipe;
using MassTransit;

namespace ExampleApp.Core.Services.Processes.Projects;

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
