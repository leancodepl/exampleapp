using ExampleApp.Core.Contracts.Projects;
using ExampleApp.Core.Domain.Events;
using LeanCode.Contracts;
using LeanPipe;
using MassTransit;

namespace ExampleApp.Core.Services.Processes.Projects;

public class PublishEmployeeUnassignedToProjectAssignmentNotification : IConsumer<EmployeeUnassignedFromAssignment>
{
    private readonly LeanPipePublisher<EmployeeAssignmentsTopic> topicPublisher;

    public PublishEmployeeUnassignedToProjectAssignmentNotification(
        LeanPipePublisher<EmployeeAssignmentsTopic> topicPublisher
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

        await topicPublisher.PublishToTopicAsync(topic, notification, context.CancellationToken);
    }
}
