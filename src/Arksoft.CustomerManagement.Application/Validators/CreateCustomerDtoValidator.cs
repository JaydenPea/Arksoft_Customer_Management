using Arksoft.CustomerManagement.Application.Common.DTOs;
using FluentValidation;

namespace Arksoft.CustomerManagement.Application.Validators;

public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters");

        RuleFor(x => x.TelephoneNumber)
            .MaximumLength(20).WithMessage("Telephone number cannot exceed 20 characters")
            .Matches(@"^[\d\s\-\+\(\)]*$").WithMessage("Please enter a valid phone number")
            .When(x => !string.IsNullOrEmpty(x.TelephoneNumber));

        RuleFor(x => x.ContactPersonName)
            .MaximumLength(100).WithMessage("Contact person name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.ContactPersonName));

        RuleFor(x => x.ContactPersonEmail)
            .EmailAddress().WithMessage("Please enter a valid email address")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.ContactPersonEmail));

        RuleFor(x => x.VatNumber)
            .MaximumLength(50).WithMessage("VAT Number cannot exceed 50 characters")
            .Matches(@"^[A-Z0-9]*$").WithMessage("VAT Number should contain only letters and numbers")
            .When(x => !string.IsNullOrEmpty(x.VatNumber));
    }
}

public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters");

        RuleFor(x => x.TelephoneNumber)
            .MaximumLength(20).WithMessage("Telephone number cannot exceed 20 characters")
            .Matches(@"^[\d\s\-\+\(\)]*$").WithMessage("Please enter a valid phone number")
            .When(x => !string.IsNullOrEmpty(x.TelephoneNumber));

        RuleFor(x => x.ContactPersonName)
            .MaximumLength(100).WithMessage("Contact person name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.ContactPersonName));

        RuleFor(x => x.ContactPersonEmail)
            .EmailAddress().WithMessage("Please enter a valid email address")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.ContactPersonEmail));

        RuleFor(x => x.VatNumber)
            .MaximumLength(50).WithMessage("VAT Number cannot exceed 50 characters")
            .Matches(@"^[A-Z0-9]*$").WithMessage("VAT Number should contain only letters and numbers")
            .When(x => !string.IsNullOrEmpty(x.VatNumber));
    }
}