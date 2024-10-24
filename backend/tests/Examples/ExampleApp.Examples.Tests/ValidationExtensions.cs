using FluentAssertions;
using FluentAssertions.Collections;
using FluentValidation.Results;
using LeanCode.CQRS.Validation.Fluent;

namespace ExampleApp.Examples.Tests;

internal static class ValidationExtensions
{
    public static AndWhichConstraint<GenericCollectionAssertions<ValidationFailure>, ValidationFailure> HaveErrorCode(
        this GenericCollectionAssertions<ValidationFailure> result,
        int errorCode
    )
    {
        return result.ContainSingle(e =>
            e.CustomState is FluentValidatorErrorState
            && ((FluentValidatorErrorState)e.CustomState).ErrorCode == errorCode
        );
    }
}
