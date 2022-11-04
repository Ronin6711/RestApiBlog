using FluentValidation;
using RestApiBlog.Contracts.V1.Requests;

namespace RestApiBlog.Validators
{
    public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
    {
        public CreatePostRequestValidator()
        {
            RuleFor(x => x.Header)
               .NotEmpty()
               .Length(5, 100);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(x => x.Image)
               .NotEmpty();
        }
    }
}
