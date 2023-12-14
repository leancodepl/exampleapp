#if Example
using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.Domain.Events;
using LeanCode.Pipe;
using MassTransit;

namespace ExampleApp.Examples.Services.Processes.Projects;

public class PublishEmployeeAssignedToProjectAssignmentNotification : IConsumer<EmployeeAssignedToAssignment>
{
    private readonly ILeanPipePublisher<EmployeeAssignmentsTopic> topicPublisher;

    public PublishEmployeeAssignedToProjectAssignmentNotification(
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
#endif
