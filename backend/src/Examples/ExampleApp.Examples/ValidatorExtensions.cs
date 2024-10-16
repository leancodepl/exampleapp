using System.Linq.Expressions;
using FluentValidation;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using LeanCode.DomainModels.Ids;
using LeanCode.DomainModels.Model;

namespace ExampleApp.Examples;

public static class ValidatorExtensions
{
    public static TypedIdValidator<TCommand> RuleForId<TCommand>(
        this AbstractValidator<TCommand> @this,
        Expression<Func<TCommand, string>> idProp
    )
    {
        return new(@this.RuleFor(idProp).Cascade(CascadeMode.Stop));
    }

    public readonly struct TypedIdValidator<TCommand>
    {
        private readonly IRuleBuilder<TCommand, string> ruleBuilder;

        internal TypedIdValidator(IRuleBuilder<TCommand, string> ruleBuilder)
        {
            this.ruleBuilder = ruleBuilder;
        }

        public AggregateValidator<TCommand, TId> IsValid<TId>(int code)
            where TId : struct, IPrefixedTypedId<TId>
        {
            var newRule = ruleBuilder
                .NotNull()
                .WithCode(code)
                .Must(TId.IsValid)
                .WithCode(code)
                .WithMessage($"Given ID is not valid {typeof(TId).Name}.");
            return new(newRule);
        }
    }

    public readonly struct AggregateValidator<TCommand, TId>
        where TId : struct, IPrefixedTypedId<TId>
    {
        private readonly IRuleBuilder<TCommand, string> ruleBuilder;

        internal AggregateValidator(IRuleBuilder<TCommand, string> ruleBuilder)
        {
            this.ruleBuilder = ruleBuilder;
        }

        public IRuleBuilderOptions<TCommand, string> Exists<TEntity>(int code)
            where TEntity : class, IAggregateRoot<TId>
        {
            return ruleBuilder
                .MustAsync(ExistsAsync<TEntity>)
                .WithCode(code)
                .WithMessage("The object with specified ID does not exist.");
        }

        public IRuleBuilderOptions<TCommand, string> DoesNotExist<TEntity>(int code)
            where TEntity : class, IAggregateRoot<TId>
        {
            return ruleBuilder
                .MustAsync(async (cmd, id, ctx, ct) => !await ExistsAsync<TEntity>(cmd, id, ctx, ct))
                .WithCode(code)
                .WithMessage("The object with specified ID already exists.");
        }

        private static async Task<bool> ExistsAsync<TEntity>(
            TCommand cmd,
            string id,
            ValidationContext<TCommand> ctx,
            CancellationToken cancellationToken
        )
            where TEntity : class, IAggregateRoot<TId>
        {
            var repository = ctx.GetService<IRepository<TEntity, TId>>();
            var typedId = TId.Parse(id);
            var entity = await repository.FindAsync(typedId, cancellationToken);
            return entity is not null;
        }
    }
}
