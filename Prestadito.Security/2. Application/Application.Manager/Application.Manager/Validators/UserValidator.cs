using FluentValidation;
using Prestadito.Security.Application.Dto.User.CreateUser;
using Prestadito.Security.Application.Dto.User.DeleteUser;
using Prestadito.Security.Application.Dto.User.DisableUser;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Application.Dto.User.UpdateUser;
using Prestadito.Security.Infrastructure.Data.Constants;

namespace Prestadito.Security.Application.Manager.Validators
{
    public class GetUserByIdValidator : AbstractValidator<GetUserByIdRequest>
    {
        public GetUserByIdValidator()
        {
            RuleFor(x => x.StrId)
                .NotEmpty().WithMessage(ConstantMessages.Errors.Validator.PROPERTY_NAME_IS_EMPTY);
        }
    }

    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.StrEmail)
                .NotEmpty().WithMessage(ConstantMessages.Errors.Validator.PROPERTY_NAME_IS_EMPTY)
                .EmailAddress().WithMessage(ConstantMessages.Errors.Validator.EMAIL_NOT_VAlID);

            RuleFor(x => x.StrPassword)
                .NotEmpty().WithMessage(ConstantMessages.Errors.Validator.PROPERTY_NAME_IS_EMPTY);

            RuleFor(x => x.StrRolId)
                .NotEmpty().WithMessage(ConstantMessages.Errors.Validator.PROPERTY_NAME_IS_EMPTY);
        }
    }

    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(ConstantMessages.Errors.Validator.PROPERTY_NAME_IS_EMPTY);

            RuleFor(x => x.StrDOI)
                .NotEmpty().WithMessage(ConstantMessages.Errors.Validator.PROPERTY_NAME_IS_EMPTY);

            RuleFor(x => x.StrPassword)
                .NotEmpty().WithMessage(ConstantMessages.Errors.Validator.PROPERTY_NAME_IS_EMPTY);

            RuleFor(x => x.StrRolId)
                .NotEmpty().WithMessage(ConstantMessages.Errors.Validator.PROPERTY_NAME_IS_EMPTY);
        }
    }

    public class DisableUserValidator : AbstractValidator<DisableUserRequest>
    {
        public DisableUserValidator()
        {
            RuleFor(x => x.StrId)
                .NotEmpty().WithMessage(ConstantMessages.Errors.Validator.PROPERTY_NAME_IS_EMPTY);
        }
    }

    public class DeleteUserValidator : AbstractValidator<DeleteUserRequest>
    {
        public DeleteUserValidator()
        {
            RuleFor(x => x.StrId)
                .NotEmpty().WithMessage(ConstantMessages.Errors.Validator.PROPERTY_NAME_IS_EMPTY);
        }
    }
}
