using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.Domain.Projects.Events;
using LeanCode.Pipe;
using MassTransit;

namespace ExampleApp.Examples.Handlers.Projects;

public class PublishEmployeeAssignedToAssignmentNotificationEH : IConsumer<EmployeeAssignedToAssignment>
{
    private readonly ILeanPipePublisher<ProjectEmployeesAssignmentsTopic> topicPublisher;

    public PublishEmployeeAssignedToAssignmentNotificationEH(
        ILeanPipePublisher<ProjectEmployeesAssignmentsTopic> topicPublisher
    )
    {
        this.topicPublisher = topicPublisher;
    }

    public Task Consume(ConsumeContext<EmployeeAssignedToAssignment> context)
    {
        var msg = context.Message;

        var topic = new ProjectEmployeesAssignmentsTopic { ProjectId = msg.ProjectId };
        var notification = new EmployeeAssignedToAssignmentDTO
        {
            AssignmentId = msg.AssignmentId,
            EmployeeId = msg.EmployeeId,
        };

        return topicPublisher.PublishAsync(topic, notification, context.CancellationToken);
    }
}
