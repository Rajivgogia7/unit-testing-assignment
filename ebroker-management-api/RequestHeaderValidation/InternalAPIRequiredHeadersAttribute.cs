using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using EBroker.Management.Application.Shared.DataError;
using System;
using System.Collections.Generic;

namespace EBroker.Management.Api.RequestHeaderValidation
{
    public sealed class InternalAPIRequiredHeadersAttribute : Attribute, IResourceFilter
    {
        public InternalAPIRequiredHeadersAttribute()
        {
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            return;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var headers = context.HttpContext.Request.Headers;
            var dataErrorDetails = new List<DataErrorDetail>();

            if (!headers.TryGetValue("correlationId", out StringValues correlationId) || string.IsNullOrWhiteSpace(correlationId))
            {
                dataErrorDetails.Add(new DataErrorDetail
                {
                    ErrorCode = DataErrorCodes.MissingCorrelationId.Code,
                    Message = DataErrorCodes.MissingCorrelationId.Description,
                    Target = "headers"
                });

            }
            if (!headers.TryGetValue("applicationId", out StringValues applicationId) || string.IsNullOrWhiteSpace(applicationId))
            {
                dataErrorDetails.Add(new DataErrorDetail
                {
                    ErrorCode = DataErrorCodes.MissingApplicationId.Code,
                    Message = DataErrorCodes.MissingApplicationId.Description,
                    Target = "headers"
                });
            }

            if (dataErrorDetails.Count > 0)
            {
                context.Result = new HeadersValidationFailureResult(dataErrorDetails);
            }
        }
    }
}
