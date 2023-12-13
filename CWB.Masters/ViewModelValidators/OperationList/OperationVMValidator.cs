using CWB.Masters.ViewModels.OperationList;
using CWB.Masters.ViewModelValidatorsMessage.OperationList;
using FluentValidation;

namespace CWB.Masters.ViewModelValidators.OperationList
{
    public class OperationVMValidator : AbstractValidator<OperationVM>
    {
        public OperationVMValidator()
        {
            RuleFor(v => v.Operation)
                   .NotEmpty().WithMessage(OperationVMValidatorMessage.EmptyOperation);
            RuleFor(v => v.TenantId)
                  .NotEmpty().WithMessage(OperationVMValidatorMessage.EmptyTenantId);
        }
    }
}
