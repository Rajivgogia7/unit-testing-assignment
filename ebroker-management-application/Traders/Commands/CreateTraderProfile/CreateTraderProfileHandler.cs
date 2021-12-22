using MediatR;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;
using EBroker.Management.Domain.Trading;
using EBroker.Management.Application.Traders.Models;
using Microsoft.EntityFrameworkCore;

namespace EBroker.Management.Application.Traders.Commands.CreateTraderProfile
{
    public class CreateTraderProfileHandler : IRequestHandler<CreateTraderProfileCommand, CreateTraderProfileResponse>
    {
        private readonly IUnitOfWork<IEBrokerDbContext> _tradingUnitOfWork;
        public CreateTraderProfileHandler(IUnitOfWork<IEBrokerDbContext> tradingUnitOfWork)
        {
            _tradingUnitOfWork = tradingUnitOfWork;
        }

        public async Task<CreateTraderProfileResponse> Handle(CreateTraderProfileCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateTraderProfileResponse();
            try
            {
                var traderRepo = _tradingUnitOfWork.GetRepository<TraderProfile>();

                var existingTraderProfile = await _tradingUnitOfWork.GetRepository<TraderProfile>().GetAll().FirstOrDefaultAsync(x => x.TraderCode == request.TraderCode);

                if(existingTraderProfile == null)
                {
                    _tradingUnitOfWork.BeginTransaction();

                    var traderProfile = new TraderProfile()
                    {
                        TraderId = Guid.NewGuid().ToString(),
                        TraderCode = request.TraderCode,
                        TraderName = request.TraderName.ToUpper(),
                        Funds = request.Funds
                    };
                    traderRepo.Add(traderProfile);

                    _tradingUnitOfWork.Commit();

                    response = new CreateTraderProfileResponse
                    {
                        TraderId = traderProfile.TraderId,
                        Status = TraderAccountCreationStatus.VALID
                    };
                }
                else
                {
                    response = new CreateTraderProfileResponse
                    {
                        TraderId = null,
                        Status = TraderAccountCreationStatus.INVALID
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
