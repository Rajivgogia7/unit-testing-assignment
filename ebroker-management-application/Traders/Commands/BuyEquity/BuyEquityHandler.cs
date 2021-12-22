using MediatR;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;
using EBroker.Management.Domain.Equity;
using EBroker.Management.Domain.Trading;
using EBroker.Management.Application.Traders.Models;
using Microsoft.EntityFrameworkCore;
using EBroker.Management.Application.Shared;

namespace EBroker.Management.Application.Traders.Commands.BuyEquity{
    public class BuyEquityHandler : IRequestHandler<BuyEquityCommand, BuyEquityResponse>
    {
        private readonly IUnitOfWork<IEBrokerDbContext> _tradingUnitOfWork;
        public BuyEquityHandler(IUnitOfWork<IEBrokerDbContext> tradingUnitOfWork)
        {
            _tradingUnitOfWork = tradingUnitOfWork;
        }

        public async Task<BuyEquityResponse> Handle(BuyEquityCommand request, CancellationToken cancellationToken)
        {
            var response = new BuyEquityResponse();
            var traderEquityDetails = new TraderEquity();

            try
            {
                var traderProfile = await _tradingUnitOfWork.GetRepository<TraderProfile>().GetAll().FirstOrDefaultAsync(x => x.TraderCode == request.TraderCode);
                var equityDetails = await _tradingUnitOfWork.GetRepository<Equity>().GetAll().FirstOrDefaultAsync(x => x.EquityCode == request.EquityCode);

                if (traderProfile != null & equityDetails != null)
                {
                    TimeSpan StartTime = new TimeSpan(9, 0, 0); //9 A.M
                    TimeSpan EndTime = new TimeSpan(15, 0, 0); //3 P.M

                    if ((DateTimeProvider.Now().DayOfWeek == DayOfWeek.Saturday) || (DateTimeProvider.Now().DayOfWeek == DayOfWeek.Sunday))
                    {
                        return new BuyEquityResponse { EquityCode = request.EquityCode, Quantity = request.Quantity, Status = TradingStatus.NOT_A_VALID_TIME_OR_DAY };
                    }
                    else if ((DateTimeProvider.Now().DayOfWeek != DayOfWeek.Saturday) && (DateTimeProvider.Now().DayOfWeek != DayOfWeek.Sunday) && !((DateTimeProvider.Now().TimeOfDay > StartTime) && (DateTimeProvider.Now().TimeOfDay < EndTime)))
                    {
                        return new BuyEquityResponse { EquityCode = request.EquityCode, Quantity = request.Quantity, Status = TradingStatus.NOT_A_VALID_TIME_OR_DAY };
                    }
                    else if (equityDetails.Quantity < request.Quantity)
                    {
                        return new BuyEquityResponse { EquityCode = request.EquityCode, Quantity = request.Quantity, Status = TradingStatus.EQUITY_NOT_SUFFICIENT };
                    }
                    else if (traderProfile.Funds < equityDetails.Price * request.Quantity)
                    {
                        return new BuyEquityResponse { EquityCode = request.EquityCode, Quantity = request.Quantity, Status = TradingStatus.FUNDS_NOT_SUFFICIENT };
                    }

                    _tradingUnitOfWork.BeginTransaction();

                    traderEquityDetails = await _tradingUnitOfWork.GetRepository<TraderEquity>().GetAll().FirstOrDefaultAsync(x => x.TraderId == traderProfile.TraderId && x.EquityId == equityDetails.EquityId);

                    if(traderEquityDetails == null)
                    {
                        var traderEquityRepo = _tradingUnitOfWork.GetRepository<TraderEquity>();
                        var traderEquityInfo = new TraderEquity()
                        {
                            TraderEquityId = Guid.NewGuid().ToString(),
                            TraderId = traderProfile.TraderId,
                            EquityId = equityDetails.EquityId,
                            Quantity = request.Quantity
                        };

                        traderEquityRepo.Add(traderEquityInfo);
                    }
                    else if (traderEquityDetails != null)
                    {
                        traderEquityDetails.Quantity+= request.Quantity;
                    }

                    equityDetails.Quantity-= request.Quantity;
                    traderProfile.Funds-= (request.Quantity * equityDetails.Price);

                    _tradingUnitOfWork.Commit();

                    response = new BuyEquityResponse
                    {
                        EquityCode = equityDetails.EquityCode,
                        Quantity = traderEquityDetails == null ? request.Quantity : traderEquityDetails.Quantity,
                        Status = TradingStatus.SUCCESS
                    };
                }
                else
                {
                    response = new BuyEquityResponse
                    {
                        EquityCode = request.EquityCode,
                        Quantity = request.Quantity,
                        Status = TradingStatus.INVALID_DETAILS
                    };
                }
            }
            catch (Exception ex)
            {
                _tradingUnitOfWork.Rollback();

                throw;
            }

            return response;
        }
    }
}
