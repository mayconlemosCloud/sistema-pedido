using Xunit;
using Moq;
using FluentAssertions;
using API.Models;
using API.Repositories;
using API.Services;
using API.Events;
using Microsoft.Extensions.Logging;

namespace API.Tests.Services;

/// <summary>
/// Testes unitários para PedidoService
/// </summary>
public class PedidoServiceTests
{
    private readonly Mock<IPedidoRepository> _mockPedidoRepository;
    private readonly Mock<IProdutoRepository> _mockProdutoRepository;
    private readonly Mock<IEventPublisher> _mockEventPublisher;
    private readonly Mock<ILogger<PedidoService>> _mockLogger;
    private readonly PedidoService _service;

    public PedidoServiceTests()
    {
        _mockPedidoRepository = new Mock<IPedidoRepository>();
        _mockProdutoRepository = new Mock<IProdutoRepository>();
        _mockEventPublisher = new Mock<IEventPublisher>();
        _mockLogger = new Mock<ILogger<PedidoService>>();
        _service = new PedidoService(
            _mockPedidoRepository.Object,
            _mockProdutoRepository.Object,
            _mockEventPublisher.Object,
            _mockLogger.Object
        );
    }

    #region CreatePedidoAsync Tests

    [Fact]
    public async Task CreatePedidoAsync_WithValidData_ShouldCreatePedido()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var produtoId = Guid.NewGuid();
        var produto = new Produto
        {
            Id = produtoId,
            Nome = "Notebook",
            Preco = 3500m,
            QuantidadeEstoque = 10
        };

        var itens = new List<(Guid, int)> { (produtoId, 2) };

        _mockProdutoRepository
            .Setup(r => r.GetByIdAsync(produtoId))
            .ReturnsAsync(produto);

        // Act
        var result = await _service.CreatePedidoAsync(clienteId, itens);

        // Assert
        result.Should().NotBeEmpty();
        _mockPedidoRepository.Verify(r => r.AddAsync(It.IsAny<Pedido>()), Times.Once);
        _mockPedidoRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        _mockProdutoRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreatePedidoAsync_WithInsufficientStock_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var produtoId = Guid.NewGuid();
        var produto = new Produto
        {
            Id = produtoId,
            Nome = "Notebook",
            Preco = 3500m,
            QuantidadeEstoque = 1 // Apenas 1 disponível
        };

        var itens = new List<(Guid, int)> { (produtoId, 5) }; // Solicitando 5

        _mockProdutoRepository
            .Setup(r => r.GetByIdAsync(produtoId))
            .ReturnsAsync(produto);

        // Act
        var act = () => _service.CreatePedidoAsync(clienteId, itens);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("*Estoque insuficiente*");
    }

    [Fact]
    public async Task CreatePedidoAsync_WithNonExistentProduct_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var produtoId = Guid.NewGuid();

        var itens = new List<(Guid, int)> { (produtoId, 2) };

        _mockProdutoRepository
            .Setup(r => r.GetByIdAsync(produtoId))
            .ReturnsAsync((Produto?)null);

        // Act
        var act = () => _service.CreatePedidoAsync(clienteId, itens);

        // Assert
        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("*não encontrado*");
    }

    [Fact]
    public async Task CreatePedidoAsync_ShouldReduceStockCorrectly()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var produtoId = Guid.NewGuid();
        var produto = new Produto
        {
            Id = produtoId,
            Nome = "Mouse",
            Preco = 150m,
            QuantidadeEstoque = 100
        };

        var itens = new List<(Guid, int)> { (produtoId, 30) };

        _mockProdutoRepository
            .Setup(r => r.GetByIdAsync(produtoId))
            .ReturnsAsync(produto);

        // Act
        await _service.CreatePedidoAsync(clienteId, itens);

        // Assert
        produto.QuantidadeEstoque.Should().Be(70);
        _mockProdutoRepository.Verify(r => r.UpdateAsync(produto), Times.Once);
    }

    #endregion

    #region GetPedidoByIdAsync Tests

    [Fact]
    public async Task GetPedidoByIdAsync_WithValidId_ShouldReturnPedido()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var clienteId = Guid.NewGuid();
        var pedido = new Pedido
        {
            Id = pedidoId,
            ClienteId = clienteId,
            Status = "CRIADO",
            ValorTotal = 3500m,
            DataCriacao = DateTime.UtcNow,
            Itens = new List<ItemPedido>()
        };

        _mockPedidoRepository
            .Setup(r => r.GetByIdAsync(pedidoId))
            .ReturnsAsync(pedido);

        // Act
        var result = await _service.GetPedidoByIdAsync(pedidoId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(pedidoId);
        result.Status.Should().Be("CRIADO");
        result.ValorTotal.Should().Be(3500m);
    }

    [Fact]
    public async Task GetPedidoByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _mockPedidoRepository
            .Setup(r => r.GetByIdAsync(pedidoId))
            .ReturnsAsync((Pedido?)null);

        // Act
        var result = await _service.GetPedidoByIdAsync(pedidoId);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetAllPedidosAsync Tests

    [Fact]
    public async Task GetAllPedidosAsync_ShouldReturnListOfPedidos()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var pedidos = new List<Pedido>
        {
            new() { Id = Guid.NewGuid(), ClienteId = clienteId, Status = "CRIADO", ValorTotal = 3500m, Itens = new List<ItemPedido>() },
            new() { Id = Guid.NewGuid(), ClienteId = clienteId, Status = "PROCESSANDO", ValorTotal = 5000m, Itens = new List<ItemPedido>() }
        };

        _mockPedidoRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(pedidos);

        // Act
        var result = await _service.GetAllPedidosAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(p => p.Should().NotBeNull());
    }

    #endregion

    #region GetPedidosByClienteIdAsync Tests

    [Fact]
    public async Task GetPedidosByClienteIdAsync_WithValidClienteId_ShouldReturnPedidos()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var pedidos = new List<Pedido>
        {
            new() { Id = Guid.NewGuid(), ClienteId = clienteId, Status = "CRIADO", ValorTotal = 3500m, Itens = new List<ItemPedido>() },
            new() { Id = Guid.NewGuid(), ClienteId = clienteId, Status = "PROCESSANDO", ValorTotal = 5000m, Itens = new List<ItemPedido>() }
        };

        _mockPedidoRepository
            .Setup(r => r.GetByClienteIdAsync(clienteId))
            .ReturnsAsync(pedidos);

        // Act
        var result = await _service.GetPedidosByClienteIdAsync(clienteId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(p => p.ClienteId.Should().Be(clienteId));
    }

    #endregion
}
