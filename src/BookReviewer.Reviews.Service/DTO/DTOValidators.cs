using FluentValidation;

namespace BookReviewer.Reviews.Service.DTO;

public class CreateReviewDTOValidator : AbstractValidator<CreateReviewDTO>
{
    public CreateReviewDTOValidator()
    {
        RuleFor(r => r.BookId).NotEmpty();
        RuleFor(r => r.Rating).InclusiveBetween((short)1, (short)10);
        RuleFor(r => r.Text).NotEmpty().Length(1, 500);
    }
}

public class UpdateReviewDTOValidator : AbstractValidator<UpdateReviewDTO>
{
    public UpdateReviewDTOValidator()
    {
        RuleFor(r => r.ReviewId).NotEmpty();
        RuleFor(r => r.BookId).NotEmpty();
        RuleFor(r => r.Rating).InclusiveBetween((short)1, (short)10);
        RuleFor(r => r.Text).NotEmpty().Length(1, 500);
    }
}