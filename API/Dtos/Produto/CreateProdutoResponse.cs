namespace API.Dtos.Produto;

public class CreateProdutoResponse
{
    public Guid ProdutoId { get; set; }
    public string Message { get; set; } = string.Empty;
}
