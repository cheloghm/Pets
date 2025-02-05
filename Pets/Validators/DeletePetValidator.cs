using FluentValidation;
using System;

namespace Pets.Validators
{
    public class DeletePetValidator : AbstractValidator<Guid>
    {
        public DeletePetValidator()
        {
            RuleFor(id => id)
                .NotEqual(Guid.Empty).WithMessage("A valid Pet Id is required.");
        }
    }
}
