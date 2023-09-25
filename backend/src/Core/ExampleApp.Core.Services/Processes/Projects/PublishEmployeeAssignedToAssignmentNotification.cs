using ExampleApp.Core.Contracts.Projects;
using ExampleApp.Core.Domain.Events;
using LeanCode.Contracts;
using LeanCode.Pipe;
using MassTransit;

namespace ExampleApp.Core.Services.Processes.Projects;

public class PublishEmployeeAssignedToAssignmentNotification : IConsumer<EmployeeAssignedToAssignment>
{
    private readonly LeanPipePublisher<ProjectEmployeesAssignmentsTopic> topicPublisher;

    public PublishEmployeeAssignedToAssignmentNotification(
        LeanPipePublisher<ProjectEmployeesAssignmentsTopic> topicPublisher
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
