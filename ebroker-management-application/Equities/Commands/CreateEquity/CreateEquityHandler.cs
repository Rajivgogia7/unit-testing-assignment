using MediatR;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;
using EBroker.Management.Domain.Equity;
using EBroker.Management.Application.Equities.Models;
using Microsoft.EntityFrameworkCore;

namespace EBroker.Management.Application.Equities.Commands.CreateEquity
{
    public class CreateEquityHandler : IRequestHandler<CreateEquityCommand, CreateEquityResponse>
    {
        private readonly IUnitOfWork<IEBrokerDbContext> _tradingUnitOfWork;
        public CreateEquityHandler(IUnitOfWork<IEBrokerDbContext> tradingUnitOfWork)
        {
            _tradingUnitOfWork = tradingUnitOfWork;
        }

        public async Task<CreateEquityResponse> Handle(CreateEquityCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateEquityResponse();
            try
            {
                var equityRepo = _tradingUnitOfWork.GetRepository<Equity>();

                var existingEquity = await _tradingUnitOfWork.GetRepository<Equity>().GetAll().FirstOrDefaultAsync(x => x.EquityCode == request.EquityCode);

                if (existingEquity == null)
                {
                    _tradingUnitOfWork.BeginTransaction();

                    var equityInfo = new Equity()
                    {
                        EquityId = Guid.NewGuid().ToString(),
                        EquityCode = request.EquityCode,
                        EquityName = request.EquityName.ToUpper(),
                        Quantity = request.Quantity,
                        Price = request.Price
                    };
                    equityRepo.Add(equityInfo);

                    _tradingUnitOfWork.Commit();

                    response = new CreateEquityResponse
                    {
                        EquityId = equityInfo.EquityId,
                        Status = EquityCreationStatus.VALID
                    };
                }
                else
                {
                    response = new CreateEquityResponse
                    {
                        EquityId = null,
                        Status = EquityCreationStatus.INVALID
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
