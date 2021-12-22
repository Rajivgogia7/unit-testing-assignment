using System.Net;
using System.Threading.Tasks;
using EBroker.Management.Api.RequestHeaderValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EBroker.Management.Application.Traders.Queries.GetTraders;
using EBroker.Management.Application.Shared.DataError;
using EBroker.Management.Application.Shared.ExtensionMethods;
using EBroker.Management.Api.Models;
using EBroker.Management.Application.Traders.Commands.CreateTraderProfile;
using EBroker.Management.Application.Traders.Commands.BuyEquity;
using EBroker.Management.Application.Traders.Commands.SellEquity;

namespace EBroker.Management.Api.Controllers
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/")]
    [ApiController]
    public class TraderController : BaseController
    {
        private readonly IMediator _mediator;
        public TraderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("trader-profiles")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetTraderProfilesResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.InternalServerError)]
        [InternalAPIRequiredHeaders]
        public async Task<GetTraderProfilesResponse> GetTraderProfilesAsync()
        {
            return await _mediator.Send(new GetTraderProfilesQuery
            {
                CorrelationId = CorrelationId,
                ApplicationId = ApplicationId
            });
        }
        
        [HttpGet("trader-profiles/{traderCode}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetTraderProfilesResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.InternalServerError)]
        [InternalAPIRequiredHeaders]
        public async Task<GetTraderProfilesResponse> GetTraderProfilesDetailsAsync(string traderCode)
        {
            return await _mediator.Send(new GetTraderProfilesQuery
            {
                CorrelationId = CorrelationId,
                ApplicationId = ApplicationId,
                TraderCode = traderCode.TrimAndLower()
            }); ;
        }

        [HttpPost("trader-profiles/create")]
        [ProducesResponseType(typeof(CreateTraderProfileResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.InternalServerError)]
        [InternalAPIRequiredHeaders]
        public async Task<CreateTraderProfileResponse> CreateTraderProfileAsync([FromBody] CreateTraderProfileRequest request)
        {
            return await _mediator.Send(new CreateTraderProfileCommand
            {
                CorrelationId = CorrelationId,
                ApplicationId = ApplicationId,
                TraderCode = request.TraderCode.TrimAndLower(),
                TraderName = request.TraderName.TrimAndLower(),
                Funds = request.Funds
            }); ;
        }

        [HttpPost("trader-profiles/buy-equity/{traderCode}")]
        [ProducesResponseType(typeof(BuyEquityResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.InternalServerError)]
        [InternalAPIRequiredHeaders]
        public async Task<BuyEquityResponse> BuyEquityAsync(string traderCode, [FromBody] BuyEquityRequest request)
        {
            return await _mediator.Send(new BuyEquityCommand
            {
                CorrelationId = CorrelationId,
                ApplicationId = ApplicationId,
                TraderCode = traderCode.TrimAndLower(),
                EquityCode = request.EquityCode.TrimAndLower(),
                Quantity = request.Quantity
            }); ;
        }

        [HttpPost("trader-profiles/sell-equity/{traderCode}")]
        [ProducesResponseType(typeof(SellEquityResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.InternalServerError)]
        [InternalAPIRequiredHeaders]
        public async Task<SellEquityResponse> SellEquityAsync(string traderCode, [FromBody] SellEquityRequest request)
        {
            return await _mediator.Send(new SellEquityCommand
            {
                CorrelationId = CorrelationId,
                ApplicationId = ApplicationId,
                TraderCode = traderCode,
                EquityCode = request.EquityCode.TrimAndLower(),
                Quantity = request.Quantity
            }); ;
        }
    }
}
