using EBroker.Management.Api.RequestHeaderValidation;
using EBroker.Management.Application.Shared.DataError;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EBroker.Management.Api.Tests.RequestHeaderValidation
{
    public class InternalAPIRequiredHeadersAttributeTests
    {
        private readonly InternalAPIRequiredHeadersAttribute _filter;

        public InternalAPIRequiredHeadersAttributeTests()
        {
            _filter = new InternalAPIRequiredHeadersAttribute();
        }

        [Fact]
        public void OnResourceExecuting_should_add_error_when_mandatory_headers_missing_in_header()
        {
            // Arrange
            var actionContext = new ActionContext(
              new DefaultHttpContext(),
              new Microsoft.AspNetCore.Routing.RouteData(),
              new ActionDescriptor()
              );
            var filters = new List<IFilterMetadata>();
            var values = new List<IValueProviderFactory>();

            var context = new ResourceExecutingContext(actionContext, filters, values);

            // Act
            _filter.OnResourceExecuting(context);

            // Assert
            Assert.IsAssignableFrom<HeadersValidationFailureResult>(context.Result);
            var validationResult = context.Result as HeadersValidationFailureResult;
            Assert.Equal(2, validationResult.DataErrorDetails.Count());
            Assert.Collection(validationResult.DataErrorDetails, x => Assert.Contains(DataErrorCodes.MissingCorrelationId.Code, x.ErrorCode),
                                                                 x => Assert.Contains(DataErrorCodes.MissingApplicationId.Code, x.ErrorCode));

        }

        [Fact]
        public void OnResourceExecuting_should_add_error_when_mandatory_headers_are_null()
        {
            // Arrange
            var actionContext = new ActionContext(
              new DefaultHttpContext(),
              new Microsoft.AspNetCore.Routing.RouteData(),
              new ActionDescriptor()
              );
            var filters = new List<IFilterMetadata>();
            var values = new List<IValueProviderFactory>();
            string value = null;
            actionContext.HttpContext.Request.Headers.Add("correlationId", new StringValues(value));
            actionContext.HttpContext.Request.Headers.Add("applicationId", new StringValues(value));
            var context = new ResourceExecutingContext(actionContext, filters, values);

            // Act
            _filter.OnResourceExecuting(context);

            // Assert
            Assert.IsAssignableFrom<HeadersValidationFailureResult>(context.Result);
            var validationResult = context.Result as HeadersValidationFailureResult;
            Assert.Equal(2, validationResult.DataErrorDetails.Count());
            Assert.Collection(validationResult.DataErrorDetails, x => Assert.Contains(DataErrorCodes.MissingCorrelationId.Code, x.ErrorCode),
                                                                 x => Assert.Contains(DataErrorCodes.MissingApplicationId.Code, x.ErrorCode));

        }

        [Fact]
        public void OnResourceExecuting_should_add_error_when_headers_are_whitespace()
        {
            // Arrange
            var actionContext = new ActionContext(
              new DefaultHttpContext(),
              new Microsoft.AspNetCore.Routing.RouteData(),
              new ActionDescriptor()
              );
            var filters = new List<IFilterMetadata>();
            var values = new List<IValueProviderFactory>();
            string value = " ";
            actionContext.HttpContext.Request.Headers.Add("correlationId", new StringValues(value));
            actionContext.HttpContext.Request.Headers.Add("applicationId", new StringValues(value));
            var context = new ResourceExecutingContext(actionContext, filters, values);

            // Act
            _filter.OnResourceExecuting(context);

            // Assert
            Assert.IsAssignableFrom<HeadersValidationFailureResult>(context.Result);
            var validationResult = context.Result as HeadersValidationFailureResult;
            Assert.Equal(2, validationResult.DataErrorDetails.Count());
            Assert.Collection(validationResult.DataErrorDetails, x => Assert.Contains(DataErrorCodes.MissingCorrelationId.Code, x.ErrorCode),
                                                                 x => Assert.Contains(DataErrorCodes.MissingApplicationId.Code, x.ErrorCode));

        }

        [Fact]
        public void OnResourceExecuting_should_not_add_error_when_headers_are_valid()
        {
            // Arrange
            var actionContext = new ActionContext(
              new DefaultHttpContext(),
              new Microsoft.AspNetCore.Routing.RouteData(),
              new ActionDescriptor()
              );
            var filters = new List<IFilterMetadata>();
            var values = new List<IValueProviderFactory>();
            string value = " ";
            actionContext.HttpContext.Request.Headers.Add("correlationId", new StringValues("4d8faff8-4088-40c6-b6ca-6c8eb9b109de"));
            actionContext.HttpContext.Request.Headers.Add("applicationId", new StringValues("Test"));
            var context = new ResourceExecutingContext(actionContext, filters, values);

            // Act
            _filter.OnResourceExecuting(context);

            // Assert
            Assert.Null(context.Result);
        }

        [Fact]
        public void OnResourceExecuted_should_not_do_anything()
        {
            // Arrange
            var actionContext = new ActionContext(
              new DefaultHttpContext(),
              new Microsoft.AspNetCore.Routing.RouteData(),
              new ActionDescriptor()
              );
            var filters = new List<IFilterMetadata>();

            var context = new ResourceExecutedContext(actionContext, filters);

            // Act
            try
            {
                _filter.OnResourceExecuted(context);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.True(false, ex.Message);
            }
        }
    }
}
