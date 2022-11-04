using FluentValidation;
using RestApiBlog.Contracts.V1.Requests;

namespace RestApiBlog.Validators
{
    public class CreateGameRequestValidator : AbstractValidator<CreateGameRequest>
    {
        public CreateGameRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9 ]*$")
                .MaximumLength(70);

            RuleFor(x => x.Image)
               .NotEmpty();
        }
    }
}
