using Xunit;
using Moq;
using FluentAssertions;
using API.Models;
using API.Repositories;
using API.Services;
using Microsoft.Extensions.Logging;

namespace API.Tests.Services;

/// <summary>
/// Testes unitários para ProdutoService
/// </summary>
public class ProdutoServiceTests
{
    private readonly Mock<IProdutoRepository> _mockRepository;
    private readonly Mock<ILogger<ProdutoService>> _mockLogger;
    private readonly ProdutoService _service;

    public ProdutoServiceTests()
    {
        _mockRepository = new Mock<IProdutoRepository>();
        _mockLogger = new Mock<ILogger<ProdutoService>>();
        _service = new ProdutoService(_mockRepository.Object, _mockLogger.Object);
    }

    #region CreateProdutoAsync Tests

    [Fact]
    public async Task CreateProdutoAsync_WithValidData_ShouldReturnGuid()
    {
        // Arrange
        var nome = "Notebook";
        var descricao = "Notebook i7";
        var preco = 3500m;
        var quantidade = 10;

        // Act
        var result = await _service.CreateProdutoAsync(nome, descricao, preco, quantidade);

        // Assert
        result.Should().NotBeEmpty();
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Produto>()), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateProdutoAsync_ShouldSaveCorrectData()
    {
        // Arrange
        var nome = "Mouse";
        var descricao = "Mouse sem fio";
        var preco = 150m;
        var quantidade = 50;

        Produto? capturedProduto = null;
        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Produto>()))
            .Callback<Produto>(p => capturedProduto = p)
            .Returns(Task.CompletedTask);

        // Act
        await _service.CreateProdutoAsync(nome, descricao, preco, quantidade);

        // Assert
        capturedProduto.Should().NotBeNull();
        capturedProduto!.Nome.Should().Be(nome);
        capturedProduto.Descricao.Should().Be(descricao);
        capturedProduto.Preco.Should().Be(preco);
        capturedProduto.QuantidadeEstoque.Should().Be(quantidade);
    }

    #endregion

    #region GetProdutoByIdAsync Tests

    [Fact]
    public async Task GetProdutoByIdAsync_WithValidId_ShouldReturnProduto()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        var produto = new Produto
        {
            Id = produtoId,
            Nome = "Teclado",
            Descricao = "Teclado mecanico",
            Preco = 450m,
            QuantidadeEstoque = 20,
            DataCriacao = DateTime.UtcNow
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync(produtoId))
            .ReturnsAsync(produto);

        // Act
        var result = await _service.GetProdutoByIdAsync(produtoId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(produtoId);
        result.Nome.Should().Be("Teclado");
        _mockRepository.Verify(r => r.GetByIdAsync(produtoId), Times.Once);
    }

    [Fact]
    public async Task GetProdutoByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        _mockRepository
            .Setup(r => r.GetByIdAsync(produtoId))
            .ReturnsAsync((Produto?)null);

        // Act
        var result = await _service.GetProdutoByIdAsync(produtoId);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetAllProdutosAsync Tests

    [Fact]
    public async Task GetAllProdutosAsync_ShouldReturnListOfProdutos()
    {
        // Arrange
        var produtos = new List<Produto>
        {
            new() { Id = Guid.NewGuid(), Nome = "Produto 1", Preco = 100m, QuantidadeEstoque = 10 },
            new() { Id = Guid.NewGuid(), Nome = "Produto 2", Preco = 200m, QuantidadeEstoque = 20 }
        };

        _mockRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(produtos);

        // Act
        var result = await _service.GetAllProdutosAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainEquivalentOf(produtos[0]);
        result.Should().ContainEquivalentOf(produtos[1]);
    }

    [Fact]
    public async Task GetAllProdutosAsync_WhenNoProductos_ShouldReturnEmptyList()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Produto>());

        // Act
        var result = await _service.GetAllProdutosAsync();

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region UpdateProdutoAsync Tests

    [Fact]
    public async Task UpdateProdutoAsync_WithValidData_ShouldUpdateProduto()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        var produtoExistente = new Produto
        {
            Id = produtoId,
            Nome = "Notebook Old",
            Descricao = "Old",
            Preco = 2000m,
            QuantidadeEstoque = 5
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync(produtoId))
            .ReturnsAsync(produtoExistente);

        // Act
        await _service.UpdateProdutoAsync(produtoId, "Notebook New", "New Description", 3000m, 10);

        // Assert
        produtoExistente.Nome.Should().Be("Notebook New");
        produtoExistente.Descricao.Should().Be("New Description");
        produtoExistente.Preco.Should().Be(3000m);
        produtoExistente.QuantidadeEstoque.Should().Be(10);
        _mockRepository.Verify(r => r.UpdateAsync(produtoExistente), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateProdutoAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        _mockRepository
            .Setup(r => r.GetByIdAsync(produtoId))
            .ReturnsAsync((Produto?)null);

        // Act
        var act = () => _service.UpdateProdutoAsync(produtoId, "Nome", "Desc", 100m, 10);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    #endregion
}
