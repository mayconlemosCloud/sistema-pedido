namespace API.Dtos.Pedido;

public class ItemPedidoResponse
{
    public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal PrecoTotal { get; set; }
}
