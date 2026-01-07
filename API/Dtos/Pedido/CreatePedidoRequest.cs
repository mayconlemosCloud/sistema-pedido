namespace API.Dtos.Pedido;

public class CreatePedidoRequest
{
    public required Guid ClienteId { get; set; }
    public List<CreatePedidoItemRequest> Itens { get; set; } = new();
}

public class CreatePedidoItemRequest
{
    public required Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
}
