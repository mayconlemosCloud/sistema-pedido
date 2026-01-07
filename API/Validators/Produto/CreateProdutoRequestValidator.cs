using FluentValidation;
using API.Dtos.Produto;

namespace API.Validators.Produto;

/// <summary>
/// Validador para CreateProdutoRequest
/// </summary>
public class CreateProdutoRequestValidator : AbstractValidator<CreateProdutoRequest>
{
    public CreateProdutoRequestValidator()
    {
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("Nome do produto é obrigatório")
            .NotNull().WithMessage("Nome do produto é obrigatório")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres")
            .MaximumLength(255).WithMessage("Nome deve ter no máximo 255 caracteres");

        RuleFor(p => p.Preco)
            .GreaterThan(0).WithMessage("Preço deve ser maior que zero")
            .LessThanOrEqualTo(999999.99m).WithMessage("Preço deve ser menor ou igual a 999999.99");

        RuleFor(p => p.QuantidadeEstoque)
            .GreaterThanOrEqualTo(0).WithMessage("Quantidade em estoque não pode ser negativa")
            .LessThanOrEqualTo(int.MaxValue).WithMessage("Quantidade em estoque inválida");

        RuleFor(p => p.Descricao)
            .MaximumLength(1000).WithMessage("Descrição deve ter no máximo 1000 caracteres")
            .When(p => !string.IsNullOrEmpty(p.Descricao));
    }
}
