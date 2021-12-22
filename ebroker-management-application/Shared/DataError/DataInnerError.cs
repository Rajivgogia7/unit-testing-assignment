using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EBroker.Management.Application.Shared.DataError
{
    [ExcludeFromCodeCoverage]
    public sealed class DataInnerError
    {
        public DataInnerError()
        {
        }

        public DataInnerError(Exception exception)
        {
            if (exception.InnerException != null)
            {
                InnerError = new DataInnerError(exception.InnerException);
            }

            Properties = new Dictionary<string, string>
            {
                { JsonConstants.DataErrorInnerErrorMessageName, exception.Message },
                { JsonConstants.DataErrorInnerErrorTypeNameName, exception.GetType().FullName },
                { JsonConstants.DataErrorInnerErrorStackTraceName, exception.StackTrace }
            };
        }

        public DataInnerError(IDictionary<string, string> properties)
        {
            Properties = new Dictionary<string, string>(properties);
        }

        internal IDictionary<string, string> Properties { get; private set; }

        [JsonPropertyName("message")]
        public string Message
        {
            get => GetStringValue(JsonConstants.DataErrorInnerErrorMessageName);
            set => SetStringValue(JsonConstants.DataErrorInnerErrorMessageName, value);
        }

        [JsonPropertyName("typeName")]
        public string TypeName
        {
            get => GetStringValue(JsonConstants.DataErrorInnerErrorTypeNameName);
            set => SetStringValue(JsonConstants.DataErrorInnerErrorTypeNameName, value);
        }

        [JsonPropertyName("stackTrace")]
        public string StackTrace
        {
            get => GetStringValue(JsonConstants.DataErrorInnerErrorStackTraceName);
            set => SetStringValue(JsonConstants.DataErrorInnerErrorStackTraceName, value);
        }

        [JsonPropertyName("innerError")]
        public DataInnerError InnerError { get; set; }

        private string GetStringValue(string propertyKey)
        {
            if (Properties.ContainsKey(propertyKey))
            {
                return Properties[propertyKey]?.ToString();
            }

            return string.Empty;
        }
        private void SetStringValue(string propertyKey, string value)
        {
            if (!Properties.ContainsKey(propertyKey))
            {
                Properties.Add(propertyKey, value);
            }
            else
            {
                Properties[propertyKey] = value;
            }
        }
    }
}
