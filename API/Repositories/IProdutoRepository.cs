using API.Models;

namespace API.Repositories;

/// <summary>
/// Interface de repositório para Produto
/// </summary>
public interface IProdutoRepository
{
    /// <summary>
    /// Obter todos os produtos
    /// </summary>
    Task<List<Produto>> GetAllAsync();

    /// <summary>
    /// Obter um produto por ID
    /// </summary>
    Task<Produto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adicionar um novo produto
    /// </summary>
    Task AddAsync(Produto produto);

    /// <summary>
    /// Atualizar um produto existente
    /// </summary>
    Task UpdateAsync(Produto produto);

    /// <summary>
    /// Deletar um produto
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Salvar mudanças no banco de dados
    /// </summary>
    Task SaveChangesAsync();
}
