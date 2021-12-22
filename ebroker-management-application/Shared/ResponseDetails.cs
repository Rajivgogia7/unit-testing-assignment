using EBroker.Management.Application.Shared.DataError;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Application.Shared
{
    [ExcludeFromCodeCoverage]
    public class ResponseDetails<T>
    {
        public ResponseDetails()
        {
            DataErrors = new List<DataErrorDetail>();
        }

        public T Response { get; set; }
        public string ErrorMessage { get; set; }
        public ICollection<DataErrorDetail> DataErrors { get; set; }

        public bool HasDataErrors
        {
            get
            {
                return (DataErrors ?? Array.Empty<DataErrorDetail>()).Count > 0;
            }
        }
    }
}
