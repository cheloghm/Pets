using FluentValidation;
using Pets.DTO;

namespace Pets.Validators
{
    public class CreatePetValidator : AbstractValidator<PetDto>
    {
        public CreatePetValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Age)
                .InclusiveBetween(0, 100).WithMessage("Age must be between 0 and 100.");

            RuleFor(x => x.AnimalType)
                .NotEmpty().WithMessage("Animal Type is required.")
                .MaximumLength(50).WithMessage("Animal Type cannot exceed 50 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
