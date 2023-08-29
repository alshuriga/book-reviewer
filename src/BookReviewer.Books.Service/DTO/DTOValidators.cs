using FluentValidation;

namespace BookReviewer.Books.Service.DTO;

public class CreateBookDTOValidator : AbstractValidator<CreateBookDTO> 
{
    public CreateBookDTOValidator()
    {
        RuleFor(b => b.Author).NotEmpty().Length(1, 50);
        RuleFor(b => b.Title).NotEmpty().Length(1, 50);
        RuleFor(b => b.Description).NotEmpty().Length(1, 500);
    }
}

public class UpdateBookDTOValidator : AbstractValidator<UpdateBookDTO>
{
    public UpdateBookDTOValidator()
    {
        RuleFor(b => b.Id).NotEmpty();
        RuleFor(b => b.Author).NotEmpty().Length(1, 50);
        RuleFor(b => b.Title).NotEmpty().Length(1, 50);
        RuleFor(b => b.Description).NotEmpty().Length(1, 500);
    }
}