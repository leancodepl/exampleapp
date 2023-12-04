using ExampleApp.Core.Contracts.Projects;
using ExampleApp.Core.Domain.Events;
using LeanCode.Pipe;
using MassTransit;

namespace ExampleApp.Core.Services.Processes.Projects;

public class PublishEmployeeUnassignedToProjectAssignmentNotification : IConsumer<EmployeeUnassignedFromAssignment>
{
    private readonly ILeanPipePublisher<EmployeeAssignmentsTopic> topicPublisher;

    public PublishEmployeeUnassignedToProjectAssignmentNotification(
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
