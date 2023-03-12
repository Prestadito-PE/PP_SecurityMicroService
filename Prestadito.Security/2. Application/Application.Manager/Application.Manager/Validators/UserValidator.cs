using FluentValidation;
using Prestadito.Security.Application.Dto.User.CreateUser;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Application.Dto.User.UpdateUser;

namespace Prestadito.Security.Application.Manager.Validators
{
    public class GetUserByIdValidator : AbstractValidator<GetUserByIdRequest>
    {
        public GetUserByIdValidator()
        {
            RuleFor(x => x.StrId)
                .NotEmpty().WithMessage("{PropertyName} is empty");
        }
    }

    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.StrEmail)
                .NotEmpty().WithMessage("{PropertyName} is empty");

            RuleFor(x => x.StrPassword)
                .NotEmpty().WithMessage("{PropertyName} is empty");

            RuleFor(x => x.StrRolId)
                .NotEmpty().WithMessage("{PropertyName} is empty");
        }
    }

    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("{PropertyName} is empty");

            RuleFor(x => x.StrDOI)
                .NotEmpty().WithMessage("{PropertyName} is empty");

            RuleFor(x => x.StrPassword)
                .NotEmpty().WithMessage("{PropertyName} is empty");

            RuleFor(x => x.StrRolId)
                .NotEmpty().WithMessage("{PropertyName} is empty");
        }
    }
}
