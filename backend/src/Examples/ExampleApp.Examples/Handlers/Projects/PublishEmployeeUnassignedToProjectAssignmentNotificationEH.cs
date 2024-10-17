using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.Domain.Projects.Events;
using LeanCode.Pipe;
using MassTransit;

namespace ExampleApp.Examples.Handlers.Projects;

public class PublishEmployeeUnassignedToProjectAssignmentNotificationEH : IConsumer<EmployeeUnassignedFromAssignment>
{
    private readonly ILeanPipePublisher<EmployeeAssignmentsTopic> topicPublisher;

    public PublishEmployeeUnassignedToProjectAssignmentNotificationEH(
        ILeanPipePublisher<EmployeeAssignmentsTopic> topicPublisher
    )
    {
        this.topicPublisher = topicPublisher;
    }

    public async Task Consume(ConsumeContext<EmployeeUnassignedFromAssignment> context)
    {
        var msg = context.Message;

        if (msg.PreviousEmployeeId is not { } previousEmployeeId)
        {
            return;
        }

        var topic = new EmployeeAssignmentsTopic { EmployeeId = previousEmployeeId };

        var notification = new EmployeeUnassignedFromProjectAssignmentDTO
        {
            ProjectId = msg.ProjectId,
            AssignmentId = msg.AssignmentId,
        };

        await topicPublisher.PublishAsync(topic, notification, context.CancellationToken);
    }
}
