using FluentValidation;
using RestApiBlog.Contracts.V1.Requests;

namespace RestApiBlog.Validators
{
    public class UpdatePublicProfileRequestValidator : AbstractValidator<UpdatePublicProfileRequest>
    {
        public UpdatePublicProfileRequestValidator()
        {
            RuleFor(x => x.NickName)
                .NotEmpty()
                .MaximumLength(20)
                .Matches("^[a-zA-Z0-9 ]*$");

            RuleFor(x => x.Discord)
                .Length(5, 70);

            RuleFor(x => x.Status)
                .NotEmpty()
                .Length(4, 10)
                .Matches("^[a-zA-Z ]*$");

            RuleFor(x => x.Games)
                .NotEmpty();
        }
    }
}
