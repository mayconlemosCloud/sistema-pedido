namespace API.Models;

/// <summary>
/// Representa um pedido do cliente
/// </summary>
public class Pedido
{
    /// <summary>
    /// Id do pedido (Guid gerado pela aplicação)
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// ID do cliente (obrigatório)
    /// </summary>
    public Guid ClienteId { get; set; }

    /// <summary>
    /// Valor total do pedido (deve ser maior que zero)
    /// </summary>
    public decimal ValorTotal { get; set; }

    /// <summary>
    /// Status do pedido (ex: CRIADO, PROCESSANDO, ENVIADO)
    /// </summary>
    public string Status { get; set; } = "CRIADO";

    /// <summary>
    /// Data de criação do pedido
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Coleção de itens do pedido
    /// </summary>
    public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
}
