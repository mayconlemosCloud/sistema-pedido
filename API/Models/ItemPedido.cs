namespace API.Models;

/// <summary>
/// Representa um item dentro de um pedido
/// </summary>
public class ItemPedido
{
    /// <summary>
    /// Id do item (Guid gerado pela aplicação)
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// ID do pedido ao qual este item pertence
    /// </summary>
    public Guid PedidoId { get; set; }

    /// <summary>
    /// ID do produto
    /// </summary>
    public Guid ProdutoId { get; set; }

    /// <summary>
    /// Quantidade do produto neste item
    /// </summary>
    public int Quantidade { get; set; }

    /// <summary>
    /// Preço unitário do produto no momento da compra
    /// </summary>
    public decimal PrecoUnitario { get; set; }

    /// <summary>
    /// Preço total deste item (Quantidade * PrecoUnitario)
    /// </summary>
    public decimal PrecoTotal { get; set; }


    public Pedido? Pedido { get; set; }
    public Produto? Produto { get; set; }
}
