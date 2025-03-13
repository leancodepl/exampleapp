using ExampleApp.Examples.Contracts.Booking.Management;
using ExampleApp.Examples.DataAccess.Blobs;
using ExampleApp.Examples.Domain.Booking;
using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Handlers.Booking.Management;

public class CreateServiceProviderCV : AbstractValidator<CreateServiceProvider>
{
    public CreateServiceProviderCV(ServiceProviderLogoStorage logoStorage)
    {
        RuleFor(cmd => cmd.Name)
            .NotEmpty()
            .WithCode(CreateServiceProvider.ErrorCodes.NameIsNullOrEmpty)
            .MaximumLength(500)
            .WithCode(CreateServiceProvider.ErrorCodes.NameIsTooLong);

        RuleFor(cmd => cmd.Type).IsInEnum().WithCode(CreateServiceProvider.ErrorCodes.TypeIsNullOrInvalid);

        RuleFor(cmd => cmd.Description)
            .NotEmpty()
            .WithCode(CreateServiceProvider.ErrorCodes.DescriptionIsNullOrEmpty)
            .MaximumLength(10000)
            .WithCode(CreateServiceProvider.ErrorCodes.DescriptionIsTooLong);

        RuleFor(cmd => cmd.CoverPhoto)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithCode(CreateServiceProvider.ErrorCodes.CoverPhotoIsInvalid)
            .Must(logoStorage.IsValid)
            .WithCode(CreateServiceProvider.ErrorCodes.CoverPhotoIsInvalid);

        RuleFor(cmd => cmd.Thumbnail)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithCode(CreateServiceProvider.ErrorCodes.ThumbnailIsInvalid)
            .Must(logoStorage.IsValid)
            .WithCode(CreateServiceProvider.ErrorCodes.ThumbnailIsInvalid);

        RuleFor(cmd => cmd.Address)
            .NotEmpty()
            .WithCode(CreateServiceProvider.ErrorCodes.AddressIsNullOrEmpty)
            .MaximumLength(1000)
            .WithCode(CreateServiceProvider.ErrorCodes.AddressIsTooLong);

        RuleFor(cmd => cmd.Location)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithCode(CreateServiceProvider.ErrorCodes.LocationIsNull)
            .SetValidator(new LocationDTOValidator());
    }
}

public class CreateServiceProviderCH(
    IRepository<ServiceProvider, ServiceProviderId> serviceProviders,
    ServiceProviderLogoStorage logoStorage
) : ICommandHandler<CreateServiceProvider>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<CreateServiceProviderCH>();

    public Task ExecuteAsync(HttpContext context, CreateServiceProvider command)
    {
        var serviceProvider = ServiceProvider.Create(
            command.Name,
            (ServiceProviderType)command.Type,
            command.Description,
            logoStorage.PrepareForStorage(command.CoverPhoto),
            logoStorage.PrepareForStorage(command.Thumbnail),
            command.Address,
            command.Location.ToDomain(),
            command.Ratings
        );
        serviceProviders.Add(serviceProvider);

        logger.Information("ServiceProvider {ServiceProviderId} added", serviceProvider.Id);

        return Task.CompletedTask;
    }
}
