namespace API.Events;

/// <summary>
/// Evento disparado quando um pedido é criado
/// </summary>
public class PedidoCriadoEvent
{
    /// <summary>
    /// ID do pedido criado
    /// </summary>
    public Guid PedidoId { get; set; }

    /// <summary>
    /// ID do cliente
    /// </summary>
    public Guid ClienteId { get; set; }

    /// <summary>
    /// Valor total do pedido
    /// </summary>
    public decimal ValorTotal { get; set; }

    /// <summary>
    /// Data de criação do pedido
    /// </summary>
    public DateTime DataCriacao { get; set; }
}
