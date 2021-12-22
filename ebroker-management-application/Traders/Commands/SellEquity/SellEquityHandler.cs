using MediatR;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;
using EBroker.Management.Domain.Equity;
using EBroker.Management.Application.Equities;
using EBroker.Management.Domain.Trading;
using EBroker.Management.Application.Traders.Models;
using Microsoft.EntityFrameworkCore;
using EBroker.Management.Application.Shared;

namespace EBroker.Management.Application.Traders.Commands.SellEquity{
    public class SellEquityHandler : IRequestHandler<SellEquityCommand, SellEquityResponse>
    {
        private readonly IEquityService _equityService;

        private readonly IUnitOfWork<IEBrokerDbContext> _tradingUnitOfWork;
        public SellEquityHandler(IUnitOfWork<IEBrokerDbContext> tradingUnitOfWork, IEquityService equityService)
        {
            _tradingUnitOfWork = tradingUnitOfWork;
            _equityService = equityService;
        }

        public async Task<SellEquityResponse> Handle(SellEquityCommand request, CancellationToken cancellationToken)
        {
            var response = new SellEquityResponse();
            try
            {
                var traderProfile = await _tradingUnitOfWork.GetRepository<TraderProfile>().GetAll().FirstOrDefaultAsync(x => x.TraderCode == request.TraderCode);
                var equityDetails = await _tradingUnitOfWork.GetRepository<Equity>().GetAll().FirstOrDefaultAsync(x => x.EquityCode == request.EquityCode);

                if (traderProfile != null & equityDetails != null)
                {
                    var traderEquityDetails = await _tradingUnitOfWork.GetRepository<TraderEquity>().GetAll().FirstOrDefaultAsync(x => x.TraderId == traderProfile.TraderId && x.EquityId == equityDetails.EquityId);

                    TimeSpan StartTime = new TimeSpan(9, 0, 0); //9 A.M
                    TimeSpan EndTime = new TimeSpan(15, 0, 0); //3 P.M

                    if ((DateTimeProvider.Now_Sell().DayOfWeek == DayOfWeek.Saturday) || (DateTimeProvider.Now_Sell().DayOfWeek == DayOfWeek.Sunday))
                    {
                        return new SellEquityResponse { EquityCode = request.EquityCode, Quantity = request.Quantity, Status = TradingStatus.NOT_A_VALID_TIME_OR_DAY };
                    }
                    else if ((DateTimeProvider.Now_Sell().DayOfWeek != DayOfWeek.Saturday) && (DateTimeProvider.Now_Sell().DayOfWeek != DayOfWeek.Sunday) && !((DateTimeProvider.Now_Sell().TimeOfDay > StartTime) && (DateTimeProvider.Now_Sell().TimeOfDay < EndTime)))
                    {
                        return new SellEquityResponse { EquityCode = request.EquityCode, Quantity = request.Quantity, Status = TradingStatus.NOT_A_VALID_TIME_OR_DAY };
                    }
                    else if (traderEquityDetails.Quantity < request.Quantity)
                    {
                        return new SellEquityResponse { EquityCode = request.EquityCode, Quantity = request.Quantity, Status = TradingStatus.EQUITY_NOT_SUFFICIENT_TRADER };
                    }
                    else if (traderProfile.Funds < 20) //Stop the transacton if funds are less than Rs. 20
                    {
                        return new SellEquityResponse { EquityCode = request.EquityCode, Quantity = request.Quantity, Status = TradingStatus.FUNDS_NOT_SUFFICIENT_TRADER_SELL };
                    }
                   
                    if (traderEquityDetails != null)
                    {
                        _tradingUnitOfWork.BeginTransaction();

                        traderEquityDetails.Quantity-= request.Quantity;

                        equityDetails.Quantity+= request.Quantity;

                        //Adding the funds back to the traders account after deducting the brokerage amount - minimum amount is 20
                        double brokerageAmount = (0.05 / 100) * (request.Quantity * equityDetails.Price);

                        if(brokerageAmount<20)
                        {
                            brokerageAmount = 20;
                        }

                        traderProfile.Funds+= ((request.Quantity * equityDetails.Price) - brokerageAmount);

                        _tradingUnitOfWork.Commit();

                        response = new SellEquityResponse
                        {
                            EquityCode = equityDetails.EquityCode,
                            Status = TradingStatus.SUCCESS
                        };
                    }
                    else
                    {
                        response = new SellEquityResponse
                        {
                            EquityCode = request.EquityCode,
                            Quantity = request.Quantity,
                            Status = TradingStatus.EQUITY_NOT_SUFFICIENT_TRADER
                        };
                    }
                }
                else {
                    response = new SellEquityResponse
                    {
                        EquityCode = request.EquityCode,
                        Quantity = request.Quantity,
                        Status = TradingStatus.INVALID_DETAILS
                    };
                }
            }
            catch (Exception e)
            {
                _tradingUnitOfWork.Rollback();

                throw;
            }
            return response;
        }
    }
}