using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.Domain.Events;
using LeanCode.Pipe;
using MassTransit;

namespace ExampleApp.Examples.Services.Processes.Projects;

public class PublishEmployeeAssignedToAssignmentNotification : IConsumer<EmployeeAssignedToAssignment>
{
    private readonly ILeanPipePublisher<ProjectEmployeesAssignmentsTopic> topicPublisher;

    public PublishEmployeeAssignedToAssignmentNotification(
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
