using EBroker.Management.Application.Shared.DataError;
using EBroker.Management.Application.Shared.RequestParameters;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Application.Shared.Validators
{
    [ExcludeFromCodeCoverage]
    public class DefaultRequestValidator<T> : AbstractValidator<T> where T : DefaultRequestParameters
    {
        public DefaultRequestValidator(bool isAnnonymus = false)
        {
            RuleFor(c => c.CorrelationId)
                .NotEmpty()
                .WithErrorCode(DataErrorCodes.MissingCorrelationId.Code)
                .WithMessage(DataErrorCodes.MissingCorrelationId.Description);

            RuleFor(c => c.ApplicationId)
              .NotEmpty()
              .WithErrorCode(DataErrorCodes.MissingApplicationId.Code)
              .WithMessage(DataErrorCodes.MissingApplicationId.Description);

        }
    }
}
