using API.Models;
using API.Repositories;
using Microsoft.Extensions.Logging;

namespace API.Services;

/// <summary>
/// Serviço de negócio para Produtos
/// </summary>
public class ProdutoService
{
    private readonly IProdutoRepository _repository;
    private readonly ILogger<ProdutoService> _logger;

    public ProdutoService(IProdutoRepository repository, ILogger<ProdutoService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Obter todos os produtos
    /// </summary>
    public async Task<List<Produto>> GetAllProdutosAsync()
    {
        _logger.LogInformation("Buscando todos os produtos");
        return await _repository.GetAllAsync();
    }

    /// <summary>
    /// Obter um produto por ID
    /// </summary>
    public async Task<Produto?> GetProdutoByIdAsync(Guid id)
    {
        _logger.LogInformation("Buscando produto com ID: {ProdutoId}", id);
        return await _repository.GetByIdAsync(id);
    }

    /// <summary>
    /// Criar um novo produto com validações
    /// </summary>
    public async Task<Guid> CreateProdutoAsync(string nome, string descricao, decimal preco, int quantidadeEstoque)
    {
        // Validações básicas
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do produto é obrigatório", nameof(nome));

        if (preco <= 0)
            throw new ArgumentException("Preço deve ser maior que zero", nameof(preco));

        if (quantidadeEstoque < 0)
            throw new ArgumentException("Quantidade em estoque não pode ser negativa", nameof(quantidadeEstoque));

        var produto = new Produto
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Descricao = descricao,
            Preco = preco,
            QuantidadeEstoque = quantidadeEstoque,
            DataCriacao = DateTime.UtcNow
        };

        _logger.LogInformation("Criando novo produto: {ProdutoNome}", nome);
        await _repository.AddAsync(produto);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Produto criado com sucesso. ID: {ProdutoId}", produto.Id);

        return produto.Id;
    }

    /// <summary>
    /// Atualizar um produto existente
    /// </summary>
    public async Task UpdateProdutoAsync(Guid id, string nome, string descricao, decimal preco, int quantidadeEstoque)
    {
        var produto = await _repository.GetByIdAsync(id);
        if (produto == null)
            throw new KeyNotFoundException($"Produto com ID {id} não encontrado");

        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do produto é obrigatório", nameof(nome));

        if (preco <= 0)
            throw new ArgumentException("Preço deve ser maior que zero", nameof(preco));

        produto.Nome = nome;
        produto.Descricao = descricao;
        produto.Preco = preco;
        produto.QuantidadeEstoque = quantidadeEstoque;

        _logger.LogInformation("Atualizando produto: {ProdutoId}", id);
        await _repository.UpdateAsync(produto);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Produto atualizado com sucesso. ID: {ProdutoId}", id);
    }
}
