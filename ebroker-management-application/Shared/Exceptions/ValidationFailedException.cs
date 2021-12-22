using EBroker.Management.Application.Shared.DataError;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Application.Shared.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class ValidationFailedException : Exception
    {
        public IEnumerable<DataErrorDetail> Errors { get; }

        public ValidationFailedException()
        {
        }
        public ValidationFailedException(string errorMessage, IEnumerable<DataErrorDetail> errors) : base(errorMessage)
        {
            Errors = errors;
        }
        public ValidationFailedException(string message) : base(message)
        {
        }

    }
}
