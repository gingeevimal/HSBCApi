using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateCAdocumentCommandValidator : AbstractValidator<UpdateCAdocumentCommand>
    {
        public UpdateCAdocumentCommandValidator()
        {
            RuleFor(p => p.department)
              .NotEmpty().WithMessage("{department} is required.")
              .NotNull()
              .MaximumLength(50).WithMessage("{department} must not exceed 50 characters.");

            RuleFor(p => p.ordertype)
               .NotEmpty().WithMessage("{ordertype} is required.");


            //RuleFor(p => p.security)
            //    .NotEmpty().WithMessage("{security} is required.")
            //    .GreaterThan(0).WithMessage("{security} should be greater than zero.");
        }
    }
}
