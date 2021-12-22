using MediatR;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;
using EBroker.Management.Domain.Trading;
using Microsoft.EntityFrameworkCore;
using EBroker.Management.Application.Traders.Models;

namespace EBroker.Management.Application.Traders.Commands.AddFunds{
    public class AddFundsHandler : IRequestHandler<AddFundsCommand,AddFundsResponse>
    {
        private readonly IUnitOfWork<IEBrokerDbContext> _tradingUnitOfWork;
        public AddFundsHandler(IUnitOfWork<IEBrokerDbContext> tradingUnitOfWork)
        {
            _tradingUnitOfWork = tradingUnitOfWork;
        }

        public async Task<AddFundsResponse> Handle(AddFundsCommand request, CancellationToken cancellationToken)
        {
            var response = new AddFundsResponse();
            double fundsToBeAdded = 0;
            try
            {
                var traderProfile = await _tradingUnitOfWork.GetRepository<TraderProfile>().GetAll().FirstOrDefaultAsync(x => x.TraderCode == request.TraderCode);

                if (traderProfile != null)
                {
                    _tradingUnitOfWork.BeginTransaction();

                    //Above 1L charges 0.05%

                    if (request.Funds>100000)
                    {
                        fundsToBeAdded = request.Funds - ((request.Funds*0.05)/100);
                    }
                    else
                    {
                        fundsToBeAdded = request.Funds;
                    }

                    traderProfile.Funds+= fundsToBeAdded;

                    _tradingUnitOfWork.Commit();

                    response.TraderCode = traderProfile.TraderCode;
                    response.Funds = traderProfile.Funds;
                    response.Status = FundsStatus.VALID;
                }
                else
                {
                    response.Status = FundsStatus.INVALID;
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
