using FluentValidation;
using RestApiBlog.Contracts.V1.Requests;

namespace RestApiBlog.Validators
{
    public class AddedGameRequestValidator : AbstractValidator<AddedGameRequest>
    {
        public AddedGameRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9 ]*$")
                .MaximumLength(70);
        }
    }
}
