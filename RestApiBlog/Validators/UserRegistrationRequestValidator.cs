using FluentValidation;
using RestApiBlog.Contracts.V1.Requests;

namespace RestApiBlog.Validators
{
    public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
    {
        public UserRegistrationRequestValidator()
        {
            RuleFor(x => x.Email)
               .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(5, 20);
        }
    }
}
