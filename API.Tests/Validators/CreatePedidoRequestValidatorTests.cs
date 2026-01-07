using Xunit;
using FluentAssertions;
using FluentValidation.TestHelper;
using API.Dtos.Pedido;
using API.Validators.Pedido;

namespace API.Tests.Validators;

/// <summary>
/// Testes para CreatePedidoRequestValidator
/// </summary>
public class CreatePedidoRequestValidatorTests
{
    private readonly CreatePedidoRequestValidator _validator;

    public CreatePedidoRequestValidatorTests()
    {
        _validator = new CreatePedidoRequestValidator();
    }

    #region ClienteId Validation

    [Fact]
    public void Validate_WithEmptyClienteId_ShouldHaveError()
    {
        // Arrange
        var request = new CreatePedidoRequest
        {
            ClienteId = Guid.Empty,
            Itens = new List<CreatePedidoItemRequest>
            {
                new() { ProdutoId = Guid.NewGuid(), Quantidade = 1 }
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.ClienteId);
    }

    [Fact]
    public void Validate_WithValidClienteId_ShouldNotHaveError()
    {
        // Arrange
        var request = new CreatePedidoRequest
        {
            ClienteId = Guid.NewGuid(),
            Itens = new List<CreatePedidoItemRequest>
            {
                new() { ProdutoId = Guid.NewGuid(), Quantidade = 1 }
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(p => p.ClienteId);
    }

    #endregion

    #region Itens Validation

    [Fact]
    public void Validate_WithEmptyItens_ShouldHaveError()
    {
        // Arrange
        var request = new CreatePedidoRequest
        {
            ClienteId = Guid.NewGuid(),
            Itens = new List<CreatePedidoItemRequest>()
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Itens);
    }

    [Fact]
    public void Validate_WithValidItens_ShouldNotHaveError()
    {
        // Arrange
        var request = new CreatePedidoRequest
        {
            ClienteId = Guid.NewGuid(),
            Itens = new List<CreatePedidoItemRequest>
            {
                new() { ProdutoId = Guid.NewGuid(), Quantidade = 1 },
                new() { ProdutoId = Guid.NewGuid(), Quantidade = 2 }
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(p => p.Itens);
    }

    #endregion

    #region Item Quantity Validation

    [Fact]
    public void Validate_WithNegativoQuantidade_ShouldHaveError()
    {
        // Arrange
        var request = new CreatePedidoRequest
        {
            ClienteId = Guid.NewGuid(),
            Itens = new List<CreatePedidoItemRequest>
            {
                new() { ProdutoId = Guid.NewGuid(), Quantidade = -5 }
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Itens[0].Quantidade");
    }

    [Fact]
    public void Validate_WithZeroQuantidade_ShouldHaveError()
    {
        // Arrange
        var request = new CreatePedidoRequest
        {
            ClienteId = Guid.NewGuid(),
            Itens = new List<CreatePedidoItemRequest>
            {
                new() { ProdutoId = Guid.NewGuid(), Quantidade = 0 }
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Itens[0].Quantidade");
    }

    [Fact]
    public void Validate_WithValidQuantidade_ShouldNotHaveError()
    {
        // Arrange
        var request = new CreatePedidoRequest
        {
            ClienteId = Guid.NewGuid(),
            Itens = new List<CreatePedidoItemRequest>
            {
                new() { ProdutoId = Guid.NewGuid(), Quantidade = 5 }
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor("Itens[0].Quantidade");
    }

    #endregion

    #region CompleteValidation

    [Fact]
    public void Validate_WithAllValidData_ShouldBeValid()
    {
        // Arrange
        var request = new CreatePedidoRequest
        {
            ClienteId = Guid.NewGuid(),
            Itens = new List<CreatePedidoItemRequest>
            {
                new() { ProdutoId = Guid.NewGuid(), Quantidade = 1 },
                new() { ProdutoId = Guid.NewGuid(), Quantidade = 2 }
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion
}
