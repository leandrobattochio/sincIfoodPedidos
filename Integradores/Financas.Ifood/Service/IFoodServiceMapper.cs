using AutoMapper;
using Financas.Domain;
using System;

namespace Financas.Ifood.Service
{
    public class IFoodServiceMapper : Profile
    {
        public IFoodServiceMapper()
        {
            CreateMap<ObterPedidosResult, PedidoIfood>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.IdIfood, opt => opt.MapFrom(c => c.id))
                .ForMember(c => c.UltimoStatus, opt => opt.MapFrom(c => c.lastStatus))
                .ForMember(c => c.DataCriacao, opt => opt.MapFrom(c => c.createdAt))
                .ForMember(c => c.DataFinalizacao, opt => opt.MapFrom(c => c.closedAt))
                
                // Classes
                .ForMember(c => c.Origem, opt => opt.MapFrom(c => c.origin))
                .ForMember(c => c.Estabelecimento, opt => opt.MapFrom(c => c.merchant))

                .ForMember(c => c.TotalPedido, opt => opt.MapFrom(c => Convert.ToDecimal(c.payments.total.value) / 100));


            CreateMap<Origin, OrigemPedidoIfood>()
                .ForMember(c => c.Plataforma, opt => opt.MapFrom(c => c.platform))
                .ForMember(c => c.AppName, opt => opt.MapFrom(c => c.appName))
                .ForMember(c => c.AppVersion, opt => opt.MapFrom(c => c.appVersion));

            CreateMap<Merchant, EstabelecimentoIfood>()
                .ForMember(c => c.IdIfood, opt => opt.MapFrom(c => Guid.Parse(c.id)))
                .ForMember(c => c.Nome, opt => opt.MapFrom(c => c.name))
                .ForMember(c => c.Tipo, opt => opt.MapFrom(c => c.type));
        }
    }
}
