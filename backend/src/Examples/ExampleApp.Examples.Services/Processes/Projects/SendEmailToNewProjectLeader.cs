using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Events;
using ExampleApp.Examples.Domain.Projects;
using ExampleApp.Examples.Services.DataAccess;
using LeanCode.DomainModels.DataAccess;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.Processes.Projects;

public class SendEmailToNewProjectLeader : IConsumer<ProjectLeaderElected>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<SendEmailToNewProjectLeader>();

    private readonly ExamplesDbContext dbContext;

    // private readonly EmailSender emailSender;

    public SendEmailToNewProjectLeader(
        ExamplesDbContext dbContext
    // EmailSender emailSender
    )
    {
        this.dbContext = dbContext;
        // this.emailSender = emailSender;
    }

    public async Task Consume(ConsumeContext<ProjectLeaderElected> context)
    {
        var msg = context.Message;

        var emailData = await dbContext
            .Projects
            .Where(p => p.Id == msg.ProjectId)
            .Join(
                dbContext.Employees,
                p => p.ProjectLeaderId,
                e => e.Id,
                (p, e) =>
                    new
                    {
                        ProjectName = p.Name,
                        LeaderEmail = e.Email,
                        LeaderName = e.Name,
                    }
            )
            .FirstAsync(context.CancellationToken);

        // emailSender.SendNewProjectLeaderEmail(emailData);

        logger.Information(
            "New project {ProjectId} leader email sent to employee {EmployeeId}",
            msg.ProjectId,
            msg.ProjectLeaderId
        );
    }
}
