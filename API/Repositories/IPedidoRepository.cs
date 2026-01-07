using API.Models;

namespace API.Repositories;

/// <summary>
/// Interface de repositório para Pedido
/// </summary>
public interface IPedidoRepository
{
    /// <summary>
    /// Obter todos os pedidos
    /// </summary>
    Task<List<Pedido>> GetAllAsync();

    /// <summary>
    /// Obter um pedido por ID
    /// </summary>
    Task<Pedido?> GetByIdAsync(Guid id);

    /// <summary>
    /// Obter pedidos de um cliente
    /// </summary>
    Task<List<Pedido>> GetByClienteIdAsync(Guid clienteId);

    /// <summary>
    /// Adicionar um novo pedido
    /// </summary>
    Task AddAsync(Pedido pedido);

    /// <summary>
    /// Atualizar um pedido existente
    /// </summary>
    Task UpdateAsync(Pedido pedido);

    /// <summary>
    /// Deletar um pedido
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Salvar mudanças no banco de dados
    /// </summary>
    Task SaveChangesAsync();
}
