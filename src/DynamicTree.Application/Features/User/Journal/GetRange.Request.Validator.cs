using FluentValidation;

namespace DynamicTree.Application.Features.User.Journal;

public class GetRangeRequestValidator : AbstractValidator<GetRangeRequest>
{
    public GetRangeRequestValidator()
    {
        RuleFor(p => p.Skip).GreaterThanOrEqualTo(0);
        RuleFor(p => p.Take).GreaterThanOrEqualTo(0);

        When(p => p.Filter is { From: { }, To: { } },
            () =>
            {
                RuleFor(p => p.Filter!.To).GreaterThan(p => p.Filter!.From);
            });
    }
}