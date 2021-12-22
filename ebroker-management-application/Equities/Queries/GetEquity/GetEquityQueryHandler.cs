using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using EBroker.Management.Domain.Equity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EBroker.Management.Application.Equities.Queries.GetEquity
{
    public class GetEquityQueryHandler : IRequestHandler<GetEquityQuery, GetEquityResponse>
    {
        private readonly IUnitOfWork<IEBrokerDbContext> _tradingUnitOfWork;

        public GetEquityQueryHandler(IUnitOfWork<IEBrokerDbContext> tradingUnitOfWork)
        {
            _tradingUnitOfWork = tradingUnitOfWork;
        }
        public async Task<GetEquityResponse> Handle(GetEquityQuery request, CancellationToken cancellationToken)
        {
            var response = new GetEquityResponse();
            try
            {
                if (String.IsNullOrWhiteSpace(request.EquityCode))
                {
                    response.Equities = await _tradingUnitOfWork.GetRepository<Equity>().GetAll().Select(x => new EquityModel
                    {
                        EquityId = x.EquityId,
                        EquityCode = x.EquityCode,
                        EquityName = x.EquityName,
                        Quantity = x.Quantity,
                        Price = x.Price
                    }).ToListAsync();
                }
                else
                {
                    response.Equities = await _tradingUnitOfWork.GetRepository<Equity>().GetAll().Select(x => new EquityModel
                    {
                        EquityId = x.EquityId,
                        EquityCode = x.EquityCode,
                        EquityName = x.EquityName,
                        Quantity = x.Quantity,
                        Price = x.Price
                    }).Where(x => x.EquityCode == request.EquityCode).ToListAsync();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
    }
}
