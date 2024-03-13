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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PedidoId));
            CreateMap<PedidoItemDto, PedidoItem>();

            CreateMap<Pedido, PedidoDto>()
                .ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => src.Id));
            CreateMap<PedidoItem, PedidoItemDto>();

            CreateMap<NotificacaoPedidoDto, PedidoDto>().ReverseMap();
        }
    }
}
