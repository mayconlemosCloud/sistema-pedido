using FluentValidation;
using API.Dtos.Pedido;

namespace API.Validators.Pedido;

/// <summary>
/// Validador para UpdatePedidoRequest
/// </summary>
public class UpdatePedidoRequestValidator : AbstractValidator<UpdatePedidoRequest>
{
    public UpdatePedidoRequestValidator()
    {
        RuleFor(p => p.Status)
            .NotEmpty().WithMessage("Status é obrigatório")
            .NotNull().WithMessage("Status é obrigatório")
            .MaximumLength(50).WithMessage("Status deve ter no máximo 50 caracteres");

        RuleFor(p => p.Itens)
            .NotEmpty().WithMessage("Pedido deve ter pelo menos um item")
            .When(p => p.Itens != null && p.Itens.Count > 0);

        RuleForEach(p => p.Itens)
            .SetValidator(new UpdatePedidoItemRequestValidator())
            .When(p => p.Itens != null && p.Itens.Count > 0);
    }
}

/// <summary>
/// Validador para UpdatePedidoItemRequest
/// </summary>
public class UpdatePedidoItemRequestValidator : AbstractValidator<UpdatePedidoItemRequest>
{
    public UpdatePedidoItemRequestValidator()
    {
        RuleFor(i => i.ProdutoId)
            .NotEmpty().WithMessage("ProdutoId é obrigatório")
            .NotEqual(Guid.Empty).WithMessage("ProdutoId não pode ser vazio");

        RuleFor(i => i.Quantidade)
            .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero")
            .LessThanOrEqualTo(int.MaxValue).WithMessage("Quantidade inválida");
    }
}
