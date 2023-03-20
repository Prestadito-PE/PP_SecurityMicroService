using FluentValidation;
using Prestadito.Security.Application.Dto.Login;

namespace Prestadito.Security.Application.Manager.Validators
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.StrEmail)
                .NotEmpty().WithMessage("{PropertyName} is empty");

            RuleFor(x => x.StrPassword)
                .NotEmpty().WithMessage("{PropertyName} is empty");

            RuleFor(x => x.StrDeviceName)
                .NotEmpty().WithMessage("{PropertyName} is empty");
        }
    }
}
