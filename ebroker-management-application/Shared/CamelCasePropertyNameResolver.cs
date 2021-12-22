using EBroker.Management.Application.Shared.ExtensionMethods;
using FluentValidation.Internal;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace EBroker.Management.Application.Shared
{
    [ExcludeFromCodeCoverage]
    public class CamelCasePropertyNameResolver
    {
        public static string ResolvePropertyName(Type type, MemberInfo memberInfo, LambdaExpression expression)
        {
            return DefaultPropertyNameResolver(memberInfo, expression).ToCamelCase();
        }

        private static string DefaultPropertyNameResolver(MemberInfo memberInfo, LambdaExpression expression)
        {
            if (expression != null)
            {
                var chain = PropertyChain.FromExpression(expression);
                if (chain.Count > 0) return chain.ToString();
            }

            if (memberInfo != null)
            {
                return memberInfo.Name;
            }

            return null;
        }
    }
}
