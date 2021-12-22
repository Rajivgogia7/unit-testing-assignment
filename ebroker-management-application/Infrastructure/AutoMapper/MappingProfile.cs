using EBroker.Management.Application.Equities.Models;
using EBroker.Management.Application.Traders.Models;
using EBroker.Management.Domain.Equity;
using EBroker.Management.Domain.Trading;
using MapProfile = AutoMapper.Profile;

namespace EBroker.Management.Application.Infrastructure.AutoMapper
{
    public class MappingProfile : MapProfile
    {
        public MappingProfile()
        {
            CreateMap<TraderProfile, TraderDetails>();
        }
    }
}
