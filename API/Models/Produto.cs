namespace API.Models;

/// <summary>
/// Representa um produto disponível para compra
/// </summary>
public class Produto
{
    /// <summary>
    /// Id do produto (Guid gerado pela aplicação)
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Nome do produto (obrigatório)
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição do produto
    /// </summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Preço unitário do produto (deve ser maior que zero)
    /// </summary>
    public decimal Preco { get; set; }

    /// <summary>
    /// Quantidade em estoque
    /// </summary>
    public int QuantidadeEstoque { get; set; }

    /// <summary>
    /// Data de criação do produto
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}
