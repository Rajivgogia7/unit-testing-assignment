using MediatR;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;
using EBroker.Management.Domain.Equity;
using Microsoft.EntityFrameworkCore;
using EBroker.Management.Application.Equities.Models;

namespace EBroker.Management.Application.Equities.Commands.AddStock{
    public class AddStockHandler : IRequestHandler<AddStockCommand, AddStockResponse>
    {
        private readonly IUnitOfWork<IEBrokerDbContext> _tradingUnitOfWork;
        public AddStockHandler(IUnitOfWork<IEBrokerDbContext> tradingUnitOfWork)
        {
            _tradingUnitOfWork = tradingUnitOfWork;
        }

        public async Task<AddStockResponse> Handle(AddStockCommand request, CancellationToken cancellationToken)
        {
            var response = new AddStockResponse();
            
            try
            {
                var equityDetails = await _tradingUnitOfWork.GetRepository<Equity>().GetAll().FirstOrDefaultAsync(x => x.EquityCode == request.EquityCode);
   
                if (equityDetails != null)
                {
                    _tradingUnitOfWork.BeginTransaction();
                    
                    equityDetails.Quantity+= request.Quantity;

                    _tradingUnitOfWork.Commit();

                    response.EquityCode = equityDetails.EquityCode;
                    response.Quantity = equityDetails.Quantity;
                    response.Status = AddStockStatus.VALID;
                }
                else
                {
                    response.EquityCode = null;
                    response.Quantity = 0;
                    response.Status = AddStockStatus.INVALID;
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
