using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.Domain.Projects.Events;
using LeanCode.Pipe;
using MassTransit;

namespace ExampleApp.Examples.Services.Handlers.Projects;

public class PublishEmployeeAssignedToProjectAssignmentNotificationEH : IConsumer<EmployeeAssignedToAssignment>
{
    private readonly ILeanPipePublisher<EmployeeAssignmentsTopic> topicPublisher;

    public PublishEmployeeAssignedToProjectAssignmentNotificationEH(
        ILeanPipePublisher<EmployeeAssignmentsTopic> topicPublisher
    )
    {
        this.topicPublisher = topicPublisher;
    }

    public async Task Consume(ConsumeContext<EmployeeAssignedToAssignment> context)
    {
        var msg = context.Message;

        if (msg.EmployeeId == msg.PreviousEmployeeId)
        {
            return;
        }

        var assignmentTopic = new EmployeeAssignmentsTopic { EmployeeId = msg.EmployeeId };

        var assignmentNotification = new EmployeeAssignedToProjectAssignmentDTO
        {
            ProjectId = msg.ProjectId,
            AssignmentId = msg.AssignmentId,
        };

        await topicPublisher.PublishAsync(assignmentTopic, assignmentNotification, context.CancellationToken);

        if (msg.PreviousEmployeeId is { } previousEmployeeId)
        {
            var unassignmentTopic = new EmployeeAssignmentsTopic { EmployeeId = previousEmployeeId };

            var unassignmentNotification = new EmployeeUnassignedFromProjectAssignmentDTO
            {
                ProjectId = msg.ProjectId,
                AssignmentId = msg.AssignmentId,
            };

            await topicPublisher.PublishAsync(unassignmentTopic, unassignmentNotification, context.CancellationToken);
        }
    }
}
