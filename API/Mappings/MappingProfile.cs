using AutoMapper;
using API.Models;
using API.Dtos.Produto;
using API.Dtos.Pedido;

namespace API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProdutoRequest, Produto>();
        CreateMap<Pedido, PedidoResponse>();
        CreateMap<ItemPedido, ItemPedidoResponse>();
    }
}
