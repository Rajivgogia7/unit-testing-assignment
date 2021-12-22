using System;
using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Application.Shared.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {

        }
        public NotFoundException(Exception ex) : base(ex.Message, ex)
        {
        }

        public NotFoundException(string target)
        {
            Target = target;
        }

        public string Target { get; }
    }
}
