using LeanCode.SendGrid;
using SendGrid.Helpers.Mail;

namespace ExampleApp.Examples.IntegrationTests.Helpers;

public class SendGridRazorClientMock : SendGridRazorClient
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<SendGridRazorClientMock>();

    public SendGridRazorClientMock()
        : base(default!, default!, default!) { }

    public override Task SendEmailAsync(SendGridMessage msg, CancellationToken cancellationToken = default)
    {
        var razorMsg = msg as SendGridRazorMessage;

        logger.Debug(
            "Email {EmailModel} would be sent",
            razorMsg?.HtmlContentModel?.GetType().Name
                ?? razorMsg?.PlainTextContentModel?.GetType().Name
                ?? nameof(SendGridMessage)
        );
        return Task.CompletedTask;
    }
}
