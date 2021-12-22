using System.Net;
using System.Threading.Tasks;
using EBroker.Management.Api.RequestHeaderValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EBroker.Management.Application.Shared.DataError;
using EBroker.Management.Application.Equities.Queries.GetEquity;
using EBroker.Management.Application.Shared.ExtensionMethods;
using EBroker.Management.Application.Equities.Commands.CreateEquity;
using EBroker.Management.Api.Models;
using EBroker.Management.Application.Equities.Commands.AddStock;
using EBroker.Management.Application.Traders.Commands.BuyEquity;
using EBroker.Management.Application.Traders.Commands.SellEquity;

namespace EBroker.Management.Api.Controllers
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/")]
    [ApiController]
    public class EquityController : BaseController
    {
        private readonly IMediator _mediator;
        public EquityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("equities")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetEquityResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.InternalServerError)]
        [InternalAPIRequiredHeaders]
        public async Task<GetEquityResponse> GetEquitiesAsync()
        {
            return await _mediator.Send(new GetEquityQuery
            {
                CorrelationId = CorrelationId,
                ApplicationId = ApplicationId
            });
        }

        [HttpGet("equities/{equityCode}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetEquityResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.InternalServerError)]
        [InternalAPIRequiredHeaders]
        public async Task<GetEquityResponse> GetEquityAsync(string equityCode)
        {
            return await _mediator.Send(new GetEquityQuery
            {
                CorrelationId = CorrelationId,
                ApplicationId = ApplicationId,
                EquityCode = equityCode.TrimAndLower()
            });
        }

        [HttpPost("equities/add")]
        [ProducesResponseType(typeof(CreateEquityResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.InternalServerError)]
        [InternalAPIRequiredHeaders]
        public async Task<CreateEquityResponse> AddEquityAsync([FromBody] CreateEquityRequest request)
        {
            return await _mediator.Send(new CreateEquityCommand
            {
                CorrelationId = CorrelationId,
                ApplicationId = ApplicationId,
                EquityCode = request.EquityCode.TrimAndLower(),
                EquityName = request.EquityName.TrimAndLower(),
                Quantity = request.Quantity,
                Price = request.Price
            }); ;
        }

        [HttpPost("equities/add-stock/{equityCode}")]
        [ProducesResponseType(typeof(CreateEquityResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DataError), (int)HttpStatusCode.InternalServerError)]
        [InternalAPIRequiredHeaders]
        public async Task<AddStockResponse> AddStock(string equityCode,[FromBody] AddStockRequest request)
        {
            return await _mediator.Send(new AddStockCommand
            {
                CorrelationId = CorrelationId,
                ApplicationId = ApplicationId,
                EquityCode = equityCode.TrimAndLower(),
                Quantity = request.Quantity
            }); ;
        }
    }
}
