namespace API.Dtos.Pedido;

public class CreatePedidoResponse
{
    public Guid PedidoId { get; set; }
    public string Message { get; set; } = string.Empty;
}
