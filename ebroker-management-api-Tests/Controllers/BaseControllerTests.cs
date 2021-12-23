using EBroker.Management.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace EBroker.Management.Api.Tests.Controllers
{
    public class BaseControllerTests
    {
        private readonly TestController _controller;
        public BaseControllerTests()
        {
            _controller = new TestController
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext { }
                }
            };
            _controller.Request.Headers.Add("applicationId", new StringValues("appId"));
            _controller.Request.Headers.Add("correlationId", new StringValues("correlationId"));
        }

        [Fact]
        public void BaseController_Returns_ApplicationId_When_Available()
        {
            //Act
            var result = _controller.GetApplicationId();

            //Assert
            Assert.Equal("appId", result.ToString());
        }

        [Fact]
        public void BaseController_Returns_CorrelationId_When_Available()
        {
            //Act
            var result = _controller.GetCorrelationId();

            //Assert
            Assert.Equal("correlationId", result.ToString());
        }
    }

    public class TestController : BaseController
    {
        public string GetCorrelationId()
        {
            return CorrelationId;
        }

        public string GetApplicationId()
        {
            return ApplicationId;
        }
    }
}
