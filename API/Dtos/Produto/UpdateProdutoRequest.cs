namespace API.Dtos.Produto;

public class UpdateProdutoRequest
{
    public required string Nome { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int QuantidadeEstoque { get; set; }
}
