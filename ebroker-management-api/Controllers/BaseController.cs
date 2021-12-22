using Microsoft.AspNetCore.Mvc;
using System;

namespace EBroker.Management.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        #region Private Variables
        private string correlationId = string.Empty;
        private string applicationId = string.Empty;
        #endregion

        #region Constructor
        public BaseController()
        {

        }
        #endregion

        #region Protected Properties
        protected string CorrelationId
        {
            get
            {
                if (String.IsNullOrWhiteSpace(correlationId))
                {
                    correlationId = GetHeaderValue("correlationId");
                }
                return correlationId;
            }
        }

        protected string ApplicationId
        {
            get
            {
                if (String.IsNullOrWhiteSpace(applicationId))
                {
                    applicationId = GetHeaderValue("applicationId");
                }
                return applicationId;
            }
        }

        #endregion

        #region Private Methods
        private string GetHeaderValue(string headerKey)
        {
            var re = Request;
            var headers = re.Headers;
            if (headers.ContainsKey(headerKey))
            {
                string token = headers[headerKey];
                return token;
            }
            return null;
        }
        #endregion
    }
}
