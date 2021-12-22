using FluentValidation.Results;
using EBroker.Management.Application.Shared.DataError;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Application.Shared.ExtensionMethods
{
    [ExcludeFromCodeCoverage]
    public static class ExtensionMethods
    {
        public static List<DataErrorDetail> MapToDataError(this IEnumerable<ValidationFailure> validationFailures)
        {
            return validationFailures.Select(x =>
            {
                return new DataErrorDetail
                {
                    ErrorCode = x.ErrorCode,
                    Message = x.ErrorMessage,
                    Target = x.PropertyName
                };
            }).ToList();
        }
        public static string ToCamelCase(this string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
            {
                return s;
            }

            var chars = s.ToCharArray();

            for (var i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                var hasNext = (i + 1 < chars.Length);
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    break;
                }

                chars[i] = char.ToLower(chars[i], CultureInfo.InvariantCulture);
            }

            return new string(chars);
        }
        public static string TrimAndLower(this string str)
        {
            return str?.Trim()?.ToLower();
        }
    }
}
