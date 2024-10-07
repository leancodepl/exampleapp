using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using LeanCode.DomainModels.Ids;
using LeanCode.DomainModels.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExampleApp.Examples.Services.Tests;

public class ValidatorExtensionsTests
{
    [Fact]
    public async Task IsValid_returns_error_when_id_is_invalid()
    {
        var validator = new FakeExistsValidator();

        var result = await validator.TestValidateAsync(Context(WithoutEntity(), "invalid id"));
        result
            .ShouldHaveValidationErrorFor(x => x)
            .Should()
            .ContainSingle()
            .Which.CustomState.Should()
            .BeEquivalentTo(new { ErrorCode = 1 });
    }

    [Fact]
    public async Task IsValid_does_not_return_error_when_id_is_valid()
    {
        var validator = new FakeExistsValidator();

        var result = await validator.TestValidateAsync(Context(WithoutEntity(), FakeId.New()));
        result
            .ShouldHaveValidationErrorFor(x => x)
            .Should()
            .NotContain(t =>
                t.CustomState is FluentValidatorErrorState && ((FluentValidatorErrorState)t.CustomState).ErrorCode == 1
            );
    }

    [Fact]
    public async Task Exists_does_not_report_error_when_the_entity_is_in_the_repository()
    {
        var validator = new FakeExistsValidator();
        var (repo, id) = WithEntity();

        var result = await validator.TestValidateAsync(Context(repo, id));
        result.ShouldNotHaveValidationErrorFor(x => x);
    }

    [Fact]
    public async Task Exists_reports_error_when_entity_is_not_in_the_repository()
    {
        var validator = new FakeExistsValidator();

        var result = await validator.TestValidateAsync(Context(WithoutEntity(), FakeId.New()));
        result
            .ShouldHaveValidationErrorFor(x => x)
            .Should()
            .ContainSingle()
            .Which.CustomState.Should()
            .BeEquivalentTo(new { ErrorCode = 2 });
    }

    [Fact]
    public async Task DoesNotExist_does_not_report_error_when_the_entity_is_not_in_the_repository()
    {
        var validator = new FakeDoesNotExistValidator();

        var result = await validator.TestValidateAsync(Context(WithoutEntity(), FakeId.New()));
        result.ShouldNotHaveValidationErrorFor(x => x);
    }

    [Fact]
    public async Task DoesNotExist_reports_error_when_the_entity_is_in_the_repository()
    {
        var validator = new FakeDoesNotExistValidator();
        var (repo, id) = WithEntity();

        var result = await validator.TestValidateAsync(Context(repo, id));
        result
            .ShouldHaveValidationErrorFor(x => x)
            .Should()
            .ContainSingle()
            .Which.CustomState.Should()
            .BeEquivalentTo(new { ErrorCode = 2 });
    }

    private (FakeRepository, FakeId) WithEntity()
    {
        var repo = new FakeRepository();
        var entity = new FakeEntity { Id = FakeId.New() };
        repo.Add(entity);
        return (repo, entity.Id);
    }

    private FakeRepository WithoutEntity() => new();

    private ValidationContext<string> Context(FakeRepository repo, string instanceToValidate)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.RequestServices = new ServiceCollection()
            .AddSingleton<IRepository<FakeEntity, FakeId>>(repo)
            .BuildServiceProvider();
        return new ValidationContext<string>(instanceToValidate)
        {
            RootContextData = { [ValidationContextExtensions.HttpContextKey] = httpContext },
        };
    }
}

internal class FakeExistsValidator : AbstractValidator<string>
{
    public FakeExistsValidator()
    {
        this.RuleForId(x => x).IsValid<FakeId>(1).Exists<FakeEntity>(2);
    }
}

internal class FakeDoesNotExistValidator : AbstractValidator<string>
{
    public FakeDoesNotExistValidator()
    {
        this.RuleForId(x => x).IsValid<FakeId>(1).DoesNotExist<FakeEntity>(2);
    }
}

[TypedId(TypedIdFormat.PrefixedUlid)]
public readonly partial record struct FakeId;

internal class FakeEntity : IAggregateRoot<FakeId>
{
    public FakeId Id { get; init; }
    public DateTime DateModified { get; set; }
}

internal class FakeRepository : FakeRepositoryBase<FakeEntity, FakeId> { }
