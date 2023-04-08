using FluentValidation;
using Prestadito.Security.Application.Dto.Session.Login;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Infrastructure.Data.Constants;

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

    public class DeleteSessionValidator : AbstractValidator<DeleteSessionRequest>
    {
        public DeleteSessionValidator()
        {
            RuleFor(x => x.StrId)
                .NotEmpty().WithMessage(ConstantMessages.Validator.PROPERTY_NAME_IS_EMPTY);
        }
    }
}
