namespace API.Dtos.Pedido;

public class PedidoResponse
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public List<ItemPedidoResponse> Itens { get; set; } = new();
}
