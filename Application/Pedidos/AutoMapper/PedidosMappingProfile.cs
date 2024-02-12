using AutoMapper;
using Domain.Pedidos;
using Application.Pedidos.DTO;

namespace Application.Pedidos.AutoMapper
{
    public class PedidosMappingProfile : Profile
    {
        public PedidosMappingProfile()
        {
            CreateMap<PedidoDto, Pedido>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PedidoId))
                .ForMember(dest => dest.PedidoItems, opt => opt.MapFrom(src => src.Items));
            CreateMap<PedidoItemDto, PedidoItem>();

            CreateMap<Pedido, PedidoDto>()
                .ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.PedidoItems));
            CreateMap<PedidoItem, PedidoItemDto>();
        }
    }
}
