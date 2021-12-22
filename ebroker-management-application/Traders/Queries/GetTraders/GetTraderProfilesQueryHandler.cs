using EBroker.Management.Application.Equities;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using EBroker.Management.Domain.Equity;
using EBroker.Management.Domain.Trading;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EBroker.Management.Application.Traders.Queries.GetTraders
{
    [ExcludeFromCodeCoverage]
    public class GetTraderProfilesQueryHandler : IRequestHandler<GetTraderProfilesQuery, GetTraderProfilesResponse>
    {
        private const string _loggingPrefix = nameof(GetTraderProfilesQueryHandler);
        private readonly IEquityService _iEquityService;

        private readonly IUnitOfWork<IEBrokerDbContext> _tradingUnitOfWork;

        public GetTraderProfilesQueryHandler(IEquityService iEquityService,
                                                IUnitOfWork<IEBrokerDbContext> tradingUnitOfWork)
        {
            _iEquityService = iEquityService;
            _tradingUnitOfWork = tradingUnitOfWork;
        }
        public async Task<GetTraderProfilesResponse> Handle(GetTraderProfilesQuery request, CancellationToken cancellationToken)
        {
            var response = new GetTraderProfilesResponse();
            try
            {
                if (String.IsNullOrWhiteSpace(request.TraderCode))
                {
                    response.Traders = await _tradingUnitOfWork.GetRepository<TraderProfile>().GetAll().Select(x => new TraderProfileModel
                    {
                        TraderId = x.TraderId,
                        TraderCode = x.TraderCode,
                        TraderName = x.TraderName,
                        Funds = x.Funds
                    }).ToListAsync();
                }
                else
                {
                    response.Traders = await _tradingUnitOfWork.GetRepository<TraderProfile>().GetAll().Select(x => new TraderProfileModel
                    {
                        TraderId = x.TraderId,
                        TraderCode = x.TraderCode,
                        TraderName = x.TraderName,
                        Funds = x.Funds
                    }).Where(x => x.TraderCode == request.TraderCode).ToListAsync();
                }

                var equityDetails = new Equity();
                var traderEquityDetails = new List<TraderEquity>();

                foreach (var profile in response.Traders)
                {
                    profile.EquityDetails = new List<TraderEquityDetailsModel>();
                    traderEquityDetails = await _tradingUnitOfWork.GetRepository<TraderEquity>().GetAll().Where(x => x.TraderId == profile.TraderId).ToListAsync();

                    foreach (var traderEquityInfo in traderEquityDetails)
                    {
                        equityDetails = await _iEquityService.GetEquityDetailsById(traderEquityInfo.EquityId);

                        profile.EquityDetails.Add(new TraderEquityDetailsModel
                        {
                            EquityCode = equityDetails.EquityCode,
                            Quantity = traderEquityInfo.Quantity,
                            Price = equityDetails.Price
                        });
                    }
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
