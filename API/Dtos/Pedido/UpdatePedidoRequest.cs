namespace API.Dtos.Pedido;

public class UpdatePedidoRequest
{
    public required string Status { get; set; }
    public List<UpdatePedidoItemRequest> Itens { get; set; } = new();
}

public class UpdatePedidoItemRequest
{
    public required Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
}
