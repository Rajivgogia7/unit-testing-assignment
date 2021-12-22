using System.Net;
using System.Threading.Tasks;
using EBroker.Management.Api.RequestHeaderValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using EBroker.Management.Application.Shared.DataError;
using EBroker.Management.Application.Shared.ExtensionMethods;
using EBroker.Management.Api.Models;
using EBroker.Management.Application.Traders.Commands.AddFunds;

namespace EBroker.Management.Api.Controllers
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/")]
    [ApiController]
    public class FundsController : BaseController
    {
        private readonly IMediator _mediator;
        public FundsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("funds/add/{tradercode}")]
        [ProducesResponseType(typeof(AddFundsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.InternalServerError)]
        [InternalAPIRequiredHeaders]
        public async Task<AddFundsResponse> AddFundsAsync(string tradercode, [FromBody] AddTraderFundsRequest request)
        {
            return await _mediator.Send(new AddFundsCommand
            {
                CorrelationId = CorrelationId,
                ApplicationId = ApplicationId,
                TraderCode = tradercode.TrimAndLower(),
                Funds = request.Funds
            }); ;
        }
    }
}
