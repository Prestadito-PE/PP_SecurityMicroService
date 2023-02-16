namespace Prestadito.Security.Application.Manager.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDTO>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.StrDOI)
                .NotEmpty().WithMessage("{PropertyName} is empty");

            RuleFor(x => x.StrPassword)
                .NotEmpty().WithMessage("{PropertyName} is empty");

            RuleFor(x => x.StrRolCode)
                .NotEmpty().WithMessage("{PropertyName} is empty");
        }
    }
}
