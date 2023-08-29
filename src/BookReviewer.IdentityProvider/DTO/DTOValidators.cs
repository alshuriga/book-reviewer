using FluentValidation;

namespace BookReviewer.InentityProvider.DTO;

public class CreateUserDTOValidator : AbstractValidator<CreateUserDTO>
{
    public CreateUserDTOValidator()
    {
        RuleFor(u => u.Email).EmailAddress();
        RuleFor(u => u.Password).NotEmpty();
    }
}

public class SignInUserDTOValidator : AbstractValidator<SignInUserDTO>
{
    public SignInUserDTOValidator()
    {
        RuleFor(u => u.Email).EmailAddress();
        RuleFor(u => u.Password).NotEmpty();
    }
}