using ExampleApp.Core.Contracts.Projects;
using ExampleApp.Core.Domain.Events;
using LeanCode.Contracts;
using LeanCode.Pipe;
using MassTransit;

namespace ExampleApp.Core.Services.Processes.Projects;

public class PublishEmployeeAssignedToProjectAssignmentNotification : IConsumer<EmployeeAssignedToAssignment>
{
    private readonly LeanPipePublisher<EmployeeAssignmentsTopic> topicPublisher;

    public PublishEmployeeAssignedToProjectAssignmentNotification(
        LeanPipePublisher<EmployeeAssignmentsTopic> topicPublisher
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
