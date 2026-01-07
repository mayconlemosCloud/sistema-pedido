using Xunit;
using FluentAssertions;
using FluentValidation.TestHelper;
using API.Dtos.Produto;
using API.Validators.Produto;

namespace API.Tests.Validators;

/// <summary>
/// Testes para CreateProdutoRequestValidator
/// </summary>
public class CreateProdutoRequestValidatorTests
{
    private readonly CreateProdutoRequestValidator _validator;

    public CreateProdutoRequestValidatorTests()
    {
        _validator = new CreateProdutoRequestValidator();
    }

    #region Nome Validation

    [Fact]
    public void Validate_WithEmptyNome_ShouldHaveError()
    {
        // Arrange
        var request = new CreateProdutoRequest
        {
            Nome = "",
            Descricao = "Test",
            Preco = 100m,
            QuantidadeEstoque = 10
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Nome);
    }

    [Fact]
    public void Validate_WithNomeShortThanMinLength_ShouldHaveError()
    {
        // Arrange
        var request = new CreateProdutoRequest
        {
            Nome = "AB", // Menos de 3 caracteres
            Descricao = "Test",
            Preco = 100m,
            QuantidadeEstoque = 10
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Nome);
    }

    [Fact]
    public void Validate_WithValidNome_ShouldNotHaveError()
    {
        // Arrange
        var request = new CreateProdutoRequest
        {
            Nome = "Notebook",
            Descricao = "Test",
            Preco = 100m,
            QuantidadeEstoque = 10
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(p => p.Nome);
    }

    #endregion

    #region Preco Validation

    [Fact]
    public void Validate_WithNegativoPreco_ShouldHaveError()
    {
        // Arrange
        var request = new CreateProdutoRequest
        {
            Nome = "Notebook",
            Descricao = "Test",
            Preco = -100m,
            QuantidadeEstoque = 10
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Preco);
    }

    [Fact]
    public void Validate_WithZeroPreco_ShouldHaveError()
    {
        // Arrange
        var request = new CreateProdutoRequest
        {
            Nome = "Notebook",
            Descricao = "Test",
            Preco = 0m,
            QuantidadeEstoque = 10
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Preco);
    }

    [Fact]
    public void Validate_WithValidPreco_ShouldNotHaveError()
    {
        // Arrange
        var request = new CreateProdutoRequest
        {
            Nome = "Notebook",
            Descricao = "Test",
            Preco = 3500m,
            QuantidadeEstoque = 10
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(p => p.Preco);
    }

    #endregion

    #region QuantidadeEstoque Validation

    [Fact]
    public void Validate_WithNegativoQuantidade_ShouldHaveError()
    {
        // Arrange
        var request = new CreateProdutoRequest
        {
            Nome = "Notebook",
            Descricao = "Test",
            Preco = 100m,
            QuantidadeEstoque = -5
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.QuantidadeEstoque);
    }

    [Fact]
    public void Validate_WithZeroQuantidade_ShouldNotHaveError()
    {
        // Arrange
        var request = new CreateProdutoRequest
        {
            Nome = "Notebook",
            Descricao = "Test",
            Preco = 100m,
            QuantidadeEstoque = 0
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(p => p.QuantidadeEstoque);
    }

    #endregion

    #region CompleteValidation

    [Fact]
    public void Validate_WithAllValidData_ShouldBeValid()
    {
        // Arrange
        var request = new CreateProdutoRequest
        {
            Nome = "Notebook Dell",
            Descricao = "Notebook i7 16GB",
            Preco = 3500m,
            QuantidadeEstoque = 10
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithMultipleErrors_ShouldReturnAllErrors()
    {
        // Arrange
        var request = new CreateProdutoRequest
        {
            Nome = "",
            Descricao = "Test",
            Preco = -100m,
            QuantidadeEstoque = -10
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.Errors.Should().HaveCountGreaterThan(2);
        result.ShouldHaveValidationErrorFor(p => p.Nome);
        result.ShouldHaveValidationErrorFor(p => p.Preco);
        result.ShouldHaveValidationErrorFor(p => p.QuantidadeEstoque);
    }

    #endregion
}
