using API.Models;
using API.Repositories;
using API.Events;
using Microsoft.Extensions.Logging;

namespace API.Services;

/// <summary>
/// Serviço de negócio para Pedidos
/// </summary>
public class PedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly EventPublisher _eventPublisher;
    private readonly ILogger<PedidoService> _logger;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IProdutoRepository produtoRepository,
        EventPublisher eventPublisher,
        ILogger<PedidoService> logger)
    {
        _pedidoRepository = pedidoRepository;
        _produtoRepository = produtoRepository;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    /// <summary>
    /// Obter todos os pedidos
    /// </summary>
    public async Task<List<Pedido>> GetAllPedidosAsync()
    {
        _logger.LogInformation("Buscando todos os pedidos");
        return await _pedidoRepository.GetAllAsync();
    }

    /// <summary>
    /// Obter um pedido por ID
    /// </summary>
    public async Task<Pedido?> GetPedidoByIdAsync(Guid id)
    {
        _logger.LogInformation("Buscando pedido com ID: {PedidoId}", id);
        return await _pedidoRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Obter pedidos de um cliente
    /// </summary>
    public async Task<List<Pedido>> GetPedidosByClienteIdAsync(Guid clienteId)
    {
        _logger.LogInformation("Buscando pedidos do cliente: {ClienteId}", clienteId);
        return await _pedidoRepository.GetByClienteIdAsync(clienteId);
    }

    /// <summary>
    /// Criar um novo pedido com validações
    /// </summary>
    public async Task<Guid> CreatePedidoAsync(Guid clienteId, List<(Guid ProdutoId, int Quantidade)> itens)
    {
        // Validações básicas
        if (clienteId == Guid.Empty)
            throw new ArgumentException("ClienteId é obrigatório", nameof(clienteId));

        if (itens == null || itens.Count == 0)
            throw new ArgumentException("Pedido deve ter pelo menos um item", nameof(itens));

        // Criar o pedido
        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = clienteId,
            Status = "CRIADO",
            DataCriacao = DateTime.UtcNow,
            Itens = new List<ItemPedido>()
        };

        decimal valorTotal = 0;

        // Processar itens do pedido
        foreach (var (produtoId, quantidade) in itens)
        {
            var produto = await _produtoRepository.GetByIdAsync(produtoId);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {produtoId} não encontrado");

            if (quantidade <= 0)
                throw new ArgumentException($"Quantidade deve ser maior que zero para o produto {produtoId}", nameof(quantidade));

            if (produto.QuantidadeEstoque < quantidade)
                throw new InvalidOperationException($"Estoque insuficiente para o produto {produto.Nome}. Disponível: {produto.QuantidadeEstoque}, Solicitado: {quantidade}");

            // Criar item do pedido
            var item = new ItemPedido
            {
                Id = Guid.NewGuid(),
                PedidoId = pedido.Id,
                ProdutoId = produtoId,
                Quantidade = quantidade,
                PrecoUnitario = produto.Preco,
                PrecoTotal = produto.Preco * quantidade
            };

            pedido.Itens.Add(item);
            valorTotal += item.PrecoTotal;

            // Reduzir estoque
            produto.QuantidadeEstoque -= quantidade;
            await _produtoRepository.UpdateAsync(produto);
        }

        if (valorTotal <= 0)
            throw new InvalidOperationException("Valor total do pedido deve ser maior que zero");

        pedido.ValorTotal = valorTotal;

        // Salvar pedido
        _logger.LogInformation("Criando novo pedido para cliente: {ClienteId} com valor total: {ValorTotal}", clienteId, valorTotal);
        await _pedidoRepository.AddAsync(pedido);
        await _pedidoRepository.SaveChangesAsync();
        await _produtoRepository.SaveChangesAsync();

        // Publicar evento de pedido criado
        var @event = new PedidoCriadoEvent
        {
            PedidoId = pedido.Id,
            ClienteId = clienteId,
            ValorTotal = valorTotal,
            DataCriacao = pedido.DataCriacao
        };

        _eventPublisher.PublishEvent(@event);
        _logger.LogInformation("Pedido criado com sucesso. ID: {PedidoId}", pedido.Id);

        return pedido.Id;
    }
}
