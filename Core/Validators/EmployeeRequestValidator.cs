using AzureWebApi.Core.DTOs;
using FluentValidation;

namespace AzureWebApi.Core.Validators
{
    public class EmployeeRequestValidator : AbstractValidator<EmployeeRequestDto>
    {
        public EmployeeRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress().WithMessage("A valid email is required.");

            RuleFor(x => x.Department)
                .NotEmpty().WithMessage("Department is required.");

            RuleFor(x => x.Designation)
                .NotEmpty().WithMessage("Designation is required.");

            RuleFor(x => x.Salary)
                .GreaterThan(0).WithMessage("Salary must be greater than 0.");

            RuleFor(x => x.DateOfJoining)
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Date of joining cannot be a future date.");
        }
    }
}
