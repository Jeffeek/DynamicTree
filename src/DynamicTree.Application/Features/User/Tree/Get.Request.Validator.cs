using FluentValidation;

namespace DynamicTree.Application.Features.User.Tree;

public class GetRequestValidator : AbstractValidator<GetRequest>
{
    public GetRequestValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MaximumLength(512);
    }
}